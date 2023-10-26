$.validator.addMethod("minservicetimevalidation", function (value, element, params) {
    return value < $(params).val();
});

$.validator.unobtrusive.adapters.add("minservicetimevalidation", ['maxservicetime'], function (options) {
    options.rules['minservicetimevalidation'] = '#' + options.params.maxservicetime;
    options.messages['minservicetimevalidation'] = options.message;
});

$.validator.addMethod("maxservicetimevalidation", function (value, element, params) {
    return value > $(params).val();
});

$.validator.unobtrusive.adapters.add("maxservicetimevalidation", ['minservicetime'], function (options) {
    options.rules['maxservicetimevalidation'] = '#' + options.params.minservicetime;
    options.messages['maxservicetimevalidation'] = options.message;
});
