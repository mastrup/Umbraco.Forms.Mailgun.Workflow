angular.module("umbraco").controller("UmbracoForms.Mailgun.Template.Workflow.Controller", function ($scope, $http, $routeParams, pickerResource, mailgunTemplatePickerResource) {
    "use strict";

    function init() {
        getTemplates();
    }

    function getTemplates() {
        mailgunTemplatePickerResource.getTemplates().then(function (response) {
            $scope.templates = response;
            getDescription();
        });
    }

    $scope.$watch('setting.value', function () {
        getDescription();
    });

    function getDescription() {
        if ($scope.templates !== undefined) {
            $scope.templates.find(function (element) {
                if (element.name === $scope.setting.value) {
                    $scope.description = element.description.trim();
                }
            });
        }
    }

    init();
});