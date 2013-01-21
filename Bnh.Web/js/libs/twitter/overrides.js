
!function($) {
    "use strict"

    $.fn.popover.Constructor.prototype.leave = function (e) {
        var self = $(e.currentTarget)[this.type](this._options).data(this.type)
        var me = this;

        var hideFunc = function () {
            if (!self.options.delay || !self.options.delay.hide) return self.hide()

            self.hoverState = 'out'
            me.timeout = setTimeout(function () {
                if (self.hoverState == 'out') self.hide()
            }, self.options.delay.hide)
        }

        if (this.timeout) clearTimeout(this.timeout)

        var hide = true;

        if (this.$tip) {
            this.$tip.one("mouseenter", function () {
                hide = false;
                $(this).one("mouseleave", function () {
                    hideFunc();
                })
            });
        }

        setTimeout(function () {
            if (hide) {
                hideFunc();
            }
        }, 200);
    }

}( window.jQuery );

