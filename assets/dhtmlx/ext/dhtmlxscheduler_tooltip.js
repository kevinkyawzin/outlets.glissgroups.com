/*
@license
dhtmlxScheduler v.4.4.0 Professional Evaluation

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function (e) {
    ! function () {
        var t = e.dhtmlXTooltip = e.tooltip = {};
        t.config = {
            className: "dhtmlXTooltip tooltip",
            timeout_to_display: 50,
            timeout_to_hide: 50,
            delta_x: 15,
            delta_y: -20
        }, t.tooltip = document.createElement("div"), t.tooltip.className = t.config.className, e._waiAria.tooltipAttr(t.tooltip), t.show = function (a, i) {
            if (!e.config.touch || e.config.touch_tooltip) {
                var n = t,
                    r = this.tooltip,
                    s = r.style;
                n.tooltip.className = n.config.className;
                var d = this.position(a),
                    o = a.target || a.srcElement;
                if (!this.isTooltip(o)) {
                    var _ = d.x + (n.config.delta_x || 0),
                        l = d.y - (n.config.delta_y || 0);
                    s.visibility = "hidden", s.removeAttribute ? (s.removeAttribute("right"), s.removeAttribute("bottom")) : (s.removeProperty("right"), s.removeProperty("bottom")), s.left = "0", s.top = "0", this.tooltip.innerHTML = i, document.body.appendChild(this.tooltip);
                    var c = this.tooltip.offsetWidth,
                        h = this.tooltip.offsetHeight;
                    document.documentElement.clientWidth - _ - c < 0 ? (s.removeAttribute ? s.removeAttribute("left") : s.removeProperty("left"), s.right = document.documentElement.clientWidth - _ + 2 * (n.config.delta_x || 0) + "px") : 0 > _ ? s.left = d.x + Math.abs(n.config.delta_x || 0) + "px" : s.left = _ + "px",
                        document.documentElement.clientHeight - l - h < 0 ? (s.removeAttribute ? s.removeAttribute("top") : s.removeProperty("top"), s.bottom = document.documentElement.clientHeight - l - 2 * (n.config.delta_y || 0) + "px") : 0 > l ? s.top = d.y + Math.abs(n.config.delta_y || 0) + "px" : s.top = l + "px", e._waiAria.tooltipVisibleAttr(this.tooltip), s.visibility = "visible", this.tooltip.onmouseleave = function (t) {
                            t = t || window.event;
                            for (var a = e.dhtmlXTooltip, i = t.relatedTarget; i != e._obj && i;) i = i.parentNode;
                            i != e._obj && a.delay(a.hide, a, [], a.config.timeout_to_hide);
                        }, e.callEvent("onTooltipDisplayed", [this.tooltip, this.tooltip.event_id])
                }
            }
        }, t._clearTimeout = function () {
            this.tooltip._timeout_id && window.clearTimeout(this.tooltip._timeout_id)
        }, t.hide = function () {
            if (this.tooltip.parentNode) {
                e._waiAria.tooltipHiddenAttr(this.tooltip);
                var t = this.tooltip.event_id;
                this.tooltip.event_id = null, this.tooltip.onmouseleave = null, this.tooltip.parentNode.removeChild(this.tooltip), e.callEvent("onAfterTooltip", [t])
            }
            this._clearTimeout()
        }, t.delay = function (e, t, a, i) {
            this._clearTimeout(),
                this.tooltip._timeout_id = setTimeout(function () {
                    var i = e.apply(t, a);
                    return e = t = a = null, i
                }, i || this.config.timeout_to_display)
        }, t.isTooltip = function (e) {
            for (var t = !1; e && !t;) t = e.className == this.tooltip.className, e = e.parentNode;
            return t
        }, t.position = function (e) {
            return e = e || window.event, {
                x: e.clientX,
                y: e.clientY
            }
        }, e.attachEvent("onMouseMove", function (a, i) {
            var n = window.event || i,
                r = n.target || n.srcElement,
                s = t,
                d = s.isTooltip(r),
                o = s.isTooltipTarget && s.isTooltipTarget(r);
            if (a || d || o) {
                var _;
                if (a || s.tooltip.event_id) {
                    var l = e.getEvent(a) || e.getEvent(s.tooltip.event_id);
                    if (!l) return;
                    if (s.tooltip.event_id = l.id, _ = e.templates.tooltip_text(l.start_date, l.end_date, l), !_) return s.hide()
                }
                o && (_ = "");
                var c;
                if (_isIE) {
                    c = {
                        pageX: void 0,
                        pageY: void 0,
                        clientX: void 0,
                        clientY: void 0,
                        target: void 0,
                        srcElement: void 0
                    };
                    for (var h in c) c[h] = n[h]
                }
                if (!e.callEvent("onBeforeTooltip", [a]) || !_) return;
                s.delay(s.show, s, [c || n, _])
            } else s.delay(s.hide, s, [], s.config.timeout_to_hide)
        }), e.attachEvent("onBeforeDrag", function () {
            return t.hide(), !0
        }), e.attachEvent("onEventDeleted", function () {
            return t.hide(), !0
        })
    }()
});