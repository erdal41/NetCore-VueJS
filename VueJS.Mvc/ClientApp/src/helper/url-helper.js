export default {

    friendlySEOUrl(url) {
        url = url.toString();

        if (url.length > 100) {
            url = url.substring(0, 100);
        }

        url = url.replace("�", "I");
        url = url.replace("�", "i");
        url = url.replace("�", "g");
        url = url.replace("�", "G");
        url = url.replace("�", "c");
        url = url.replace("�", "C");
        url = url.replace("�", "o");
        url = url.replace("�", "O");
        url = url.replace("�", "s");
        url = url.replace("�", "S");
        url = url.replace("�", "u");
        url = url.replace("�", "U");
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