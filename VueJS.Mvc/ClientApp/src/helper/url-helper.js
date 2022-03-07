export default {

    friendlySEOUrl(url) {
        url = url.toString();

        if (url.length > 100) {
            url = url.substring(0, 100);
        }

        url = url.replace("İ", "I");
        url = url.replace("ı", "i");
        url = url.replace("ğ", "g");
        url = url.replace("Ğ", "G");
        url = url.replace("ç", "c");
        url = url.replace("Ç", "C");
        url = url.replace("ö", "o");
        url = url.replace("Ö", "O");
        url = url.replace("ş", "s");
        url = url.replace("Ş", "S");
        url = url.replace("ü", "u");
        url = url.replace("Ü", "U");
        url = url.replace("'", "");
        url = url.replace("\"", "");
        url = url.replace("&", "");

        var chars = '$%#@!*?;:~`+=()[]{}|\'<>,/^&"".';
        var charArray = [];
        charArray.push(chars.split(''));
        for (var i = 0; i < charArray.length; i++) {
            var strChr = charArray[i].toString();
            if (url.includes(strChr)) {
                url = url.replace(strChr, '');
            }
        }

        url = url.normalize('NFD')
            .replace(/[\u0300-\u036f]/g, '')
            .replace(/\s+/g, '-')
            .toLowerCase()
            .trim()
            .replace(/&/g, '-and-')
            .replace(/[^a-z0-9\-]/g, '')
            .replace(/-+/g, '-')
            .replace(/^-*/, '')
            .replace(/-*$/, '');
        return url;
    }
}