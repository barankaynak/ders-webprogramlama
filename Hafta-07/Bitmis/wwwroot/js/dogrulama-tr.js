// HAFTA 6 — jQuery Validation'a Türkçe sayı biçimini öğretir
// (Hafta 6 ders notu bölüm 4.4).
//
// Kütüphane sayıyı İngilizce biçimde bekler: "120.50". Sunucu ise tr-TR'de
// "120,50" bekler (Program.cs). Bu dosya olmadan ölçülen:
//   "120,50" → istemci reddeder: "must be a number" — doğru değer gönderilemez
//   "12.50"  → istemci geçirir, sunucu 1250 okur    — veri sessizce bozulur
//
// Kabul edilen biçim: 120,50 · 1.250,50 · 99 · -3,5   (nokta binlik, virgül ondalık)
(function ($) {
    if (!$ || !$.validator) return;

    var TR = /^-?(?:\d+|\d{1,3}(?:\.\d{3})+)(?:,\d+)?$/;

    function sayiya(metin) {
        return parseFloat(metin.replace(/\./g, '').replace(',', '.'));
    }

    $.validator.methods.number = function (value, element) {
        return this.optional(element) || TR.test(value);
    };

    $.validator.methods.range = function (value, element, param) {
        if (this.optional(element)) return true;
        var n = TR.test(value) ? sayiya(value) : NaN;
        return n >= Number(param[0]) && n <= Number(param[1]);
    };
})(window.jQuery);
