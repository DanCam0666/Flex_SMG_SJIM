$(function () {
    //Bootstrap datepicker plugin
    $('#bs_datepicker_container input').datepicker({
        autoclose: true,
        container: '#bs_datepicker_container',
       language: "es",
        calendarWeeks: true,
        todayHighlight: true,
        forceParse:true,
        format: "dd/mm/yyyy"

    });

    $('#bs_datepicker_component_container').datepicker({
        autoclose: true,
        container: '#bs_datepicker_component_container',
        language: "es",
        calendarWeeks: true,
        todayHighlight: true,
        forceParse: true,
        format: "dd/mm/yyyy"
    });
    
    $('#bs_datepicker_range_container').datepicker({
        autoclose: true,
        container: '#bs_datepicker_range_container',
        language: "es",
        calendarWeeks: true,
        todayHighlight: true,
        forceParse: true,
        format: "dd/mm/yyyy"
    });

    $.validator.addMethod('date',
            function (value, element, params) {
                return true;
            });

});

