$(function () {
    "use strict";

    function ViewModel() {
        var self = this;
        self.teamCityUrl = ko.observable("");
        self.teamCityPassword = ko.observable("");
        self.teamCityUserName = ko.observable("");
        self.projects = ko.observableArray();
        self.hideProject = function(project) {
            $.ajax({
                url: "/tc/project/" + project.Id  + "/hide",
                method: "POST"
            }).done(function() {
                var index = self.projects().indexOf(project);
                self.projects().splice(index, 1);
                self.projects(self.projects());
            });
        };
        self.hideBuildType = function(buildType) {
            $.ajax({
                url: "/tc/buildtype/" + buildType.BuildTypeId + "/hide",
                method: "POST"
            }).done(function() {

                var row = Enumerable.From(model.projects())
                    .SelectMany(function(p) {
                        return p.BuildTypeRows;
                    })
                    .Where(function(br) {
                        return br.BuildTypes.indexOf(buildType) > -1;
                    }).FirstOrDefault();

                var index = row.BuildTypes.indexOf(buildType);
                row.BuildTypes.splice(index, 1);
                var copy = self.projects();
                self.projects([]);
                self.projects(copy);
            });
        };
    }

    var model = new ViewModel();
    ko.applyBindings(model);

    $.ajax({
        url: "/admin/config/tc",
        method: "GET"
    }).done(function (data) {
        if (data) {

            if (!data.Uri) {
                return;
            }
            getModel().subscribe(updateModel,
                function(err) { console.log(err); },
                function() {
                    model.teamCityUrl(data.Uri);
                    model.teamCityPassword(data.Password);
                    model.teamCityUserName(data.UserName);
                    refreshBuildTypesForModel();
                    getRunningBuilds().retry().subscribe(updateModelWithRunningBuilds, function(err) {
                        console.log(err);
                    });
                }
            );
        }
    });

    $("#submit").on("click", function (e) {
        e.preventDefault();
        $.ajax({
            url: "/admin/config/tc",
            method: "POST",
            data: { Uri: $("#url").val(), UserName: $("#userName").val(), Password: $("#password").val() }
        }).done(function () {
            getModel().subscribe(updateModel,
                function (err) { console.log(err); },
                function () {
                    refreshBuildTypesForModel();
                    getRunningBuilds().retry().subscribe(updateModelWithRunningBuilds, function (err) {
                        console.log(err);
                    });
                }
            );
        });
    });
    
    
    function updateModel(data) {
        if (!data.Projects) {
            return;
        }
        if (data.Projects.length > 0) {
            model.projects(data.Projects);
        }
    }

    function updateModelWithRunningBuilds (data) {
        if (!data.Builds) {
            return;
        }

        data.Builds.forEach(function (build) {
            if (build.Status === "SUCCESS") {
                build.Status = "RUNNING";
            } else {
                build.Status = "RUNNING ERRORS";
            }

            getBuildDetail(build.Id).subscribe(function (b) {
                if (!b) {
                    return;
                }
                if (b.RunningInfo) {
                    b.Status = build.Status;
                    b.PercentageComplete = b.RunningInfo.PercentageComplete;
                }
                updateModelWithBuild(b);
            });
        });

        refreshBuildTypesForModel(data.Builds);
    }
    
    function updateModelWithBuild(build) {
        if (!build) {
            return;
        }
        if (build.StartDate) {
            build.StartDate = moment(build.StartDate, 'YYYYMMDDTHH:mmssZ').calendar();
        }

        var buildType = Enumerable.From(model.projects())
            .SelectMany(function (p) {
                return p.BuildTypeRows;
            })
            .SelectMany(function (br) {
                return br.BuildTypes;
            })
            .Where(function (bt) {
                return bt.BuildTypeId === build.BuildTypeId;
            }).FirstOrDefault();

        if (!buildType) {
            return;
        }

        buildType.LastBuild = build;
        var copy = model.projects();
        model.projects([]);
        model.projects(copy);
    }

    function getRunningBuilds() {
        return Rx.Observable.timer(10000, 30000)
            .selectMany(function() {
                return Rx.Observable.fromPromise($.ajax({
                    url: "/tc/runningbuilds"
                }).promise());
            });
    }
   
    function getModel() {
        var promise = $.ajax({
            url: "/tc/model"
        }).promise();
        return Rx.Observable.fromPromise(promise);
    }

    function getBuildDetail(buildId) {
        var promise = $.ajax({
            url: "/tc/build/" + buildId
        }).promise();
        return Rx.Observable.fromPromise(promise);
    }

    function getLastBuilds(buildTypeIds) {
        var promise = $.ajax({
            url: "/tc/buildType/builds",
            data: { BuildTypeIds: buildTypeIds },
            method: "POST"
        }).promise();
        return Rx.Observable.fromPromise(promise);
    }

    function refreshBuildTypesForModel(runningBuilds) {
        var runningBuildTypes = [];
        var nonRunningBuildTypes = [];
        runningBuilds = runningBuilds || [];

        Enumerable.From(model.projects())
            .ForEach(function (p) {
                Enumerable.From(p.BuildTypeRows)
                    .ForEach(function (bt) {
                        Enumerable.From(bt.BuildTypes)
                            .ForEach(function (b) {
                                if (b.LastBuild.Status === "RUNNING" || b.LastBuild.Status === "RUNNING ERRORS") {
                                    runningBuildTypes.push(b);
                                } else {
                                    nonRunningBuildTypes.push(b);
                                }
                            });
                    });
            });

        var lastBuildsToGet = [];

        runningBuildTypes.forEach(function (bt) {
            bt.NextUpdate = moment();
        });

        var completedBuildTypes = Enumerable.From(runningBuildTypes)
            .Where(function(bt) {
                return !Enumerable.From(runningBuilds)
                    .Any(function(b) {
                        return b.BuildTypeId === bt.BuildTypeId;
                    });
            }).ToArray();

        completedBuildTypes.forEach(function(bt) {
            lastBuildsToGet.push(bt.BuildTypeId);
        });

        nonRunningBuildTypes.forEach(function (b) {
            if (!b.NextUpdate || moment().isAfter(b.NextUpdate)) {
                lastBuildsToGet.push(b.BuildTypeId);
                b.NextUpdate = moment().add("hours", 1);
            }
        });

        if (lastBuildsToGet.length > 0) {
            getLastBuilds(lastBuildsToGet).subscribe(function(data) {
                if (!data) {
                    return;
                }
                data.forEach(function(b) {
                    setTimeout(function () {
                        updateModelWithBuild(b);
                    }, 0);
                    
                });
            });
        }
    }

    moment.lang("en", {
        calendar: {
            lastDay: '[Yesterday at] LT',
            sameDay: '[Today at] LT',
            nextDay: '[Tomorrow at] LT',
            lastWeek: 'dddd [at] LT',
            nextWeek: 'dddd [at] LT',
            sameElse: "ll"
        }
    });

    
});


