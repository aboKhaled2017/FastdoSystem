"use strict";
var JqueyExtenionsObj = {
    setLoading: function (icon) {
        var el = this;
        el.attr('disabled', 'disabled');
        var i = el.find('i');
        i.removeClass(icon).addClass('fa-circle-o-notch fa-spin');
        i.data('icon', icon);
        return el;
    },
    removeLoading: function () {
        var el = this;
        el.removeAttr('disabled');
        var i = el.find('i');
        var icon = i.data('icon');
        i.addClass(icon).removeClass('fa-circle-o-notch fa-spin');
        return el;
    }
};
function delay(callback, ms) {
    var timer = 0;
    return function () {
        var context = this, args = arguments;
        clearTimeout(timer);
        timer = setTimeout(function () {
            callback.apply(context, args);
        }, ms || 0);
    };
}
jQuery.fn.extend(JqueyExtenionsObj);
//# sourceMappingURL=Utiles.js.map