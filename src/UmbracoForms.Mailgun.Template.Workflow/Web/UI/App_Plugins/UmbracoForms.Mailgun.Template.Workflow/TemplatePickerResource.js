angular.module('umbraco.resources').factory('mailgunTemplatePickerResource', function ($http,umbRequestHelper) {
    var root = '/Umbraco/backoffice/api/ListTemplatesApi/';
    return {
        getTemplates: function() {
            return umbRequestHelper.resourcePromise($http.get(root + 'GetTemplates'), 'No templates found.');
        }
    };
});