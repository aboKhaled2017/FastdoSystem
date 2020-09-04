interface JQuery {
    setLoading: (icon: string) => JQuery<HTMLElement>
    removeLoading: () => JQuery<HTMLElement>
}
const JqueyExtenionsObj = {
    setLoading: function (icon: string) {
        var el = (this as any as JQuery<HTMLElement>);
        el.attr('disabled', 'disabled');
        var i = el.find('i');
        i.removeClass(icon).addClass('fa-circle-o-notch fa-spin');
        i.data('icon', icon);
        return el;
    },
    removeLoading: function () {
        var el = (this as JQuery<HTMLElement>);
        el.removeAttr('disabled');
        var i = el.find('i');
        var icon = i.data('icon');
        i.addClass(icon).removeClass('fa-circle-o-notch fa-spin');
        return el;
    }
}
function delay(callback: Function, ms: number) {
    var timer = 0;
    return function (this: (e:any)=>void) {
        var context = this, args = arguments;
        clearTimeout(timer);
        timer = setTimeout(function () {
            callback.apply(context, args);
        }, ms || 0);
    };
}
jQuery.fn.extend(JqueyExtenionsObj);