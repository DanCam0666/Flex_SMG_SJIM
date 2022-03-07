(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? module.exports = factory() :
    typeof define === 'function' && define.amd ? define(factory) :
    (global = global || self, (global.FullCalendarLocales = global.FullCalendarLocales || {}, global.FullCalendarLocales.sk = factory()));
}(this, (function () { 'use strict';

    var sk = {
        code: "sk",
        week: {
            dow: 1,
            doy: 4 // The week that contains Jan 4th is the first week of the year.
        },
        buttonText: {
            prev: "Anterior",
            next: "Próxima",
            today: "Hoy",
            month: "Mes",
            week: "Semana",
            day: "Día",
            list: "Calendario"
        },
        weekLabel: "Tú",
        allDayText: "Todo el Días",
        eventLimitText: function (n) {
            return "+Próxima: " + n;
        },
        noEventsMessage: "No hay acciones para mostrar"
    };

    return sk;

})));
