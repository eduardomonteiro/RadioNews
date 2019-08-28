//Para fazer a validação de data no cliente funcionar com formato pt-br
//Ricardo Baltazar Chaves
//
jQuery.extend(jQuery.validator.methods, {
    date: function (value, element) {
        //return this.optional(element) || /^\d\d?\/\d\d?\/\d\d\d?\d?$/.test(value);
        return this.optional(element) || /^\d\d?\/\d\d?\/\d\d\d?\d?$/.test(value);
    },
    number: function (value, element) {
        return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:\.\d{3})+)(?:,\d+)?$/.test(value);
    }
});