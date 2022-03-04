// eslint-disable-next-line no-mixed-spaces-and-tabs
		
function init_morris_charts() {
			
			if( typeof (Morris) === 'undefined'){ return; }
			console.log('init_morris_charts');
			
			if ($('#graph_bar').length){ 
			
				Morris.Bar({
				  element: 'graph_bar',
				  data: [
                      { Algo: 'a', valor: 380},
                      { Algo: 'b', valor: 655},
                      { Algo: 'c', valor: 275},
                      { Algo: 'd', valor: 1571},
                      { Algo: 'e', valor: 655},
				  ],
                    xkey: 'Algo',
                    ykeys: ['valor'],
                    labels: ['Valor'],
				  barRatio: 0.4,
				  barColors: ['#26B99A', '#34495E', '#ACADAC', '#3498DB'],
				  xLabelAngle: 35,
				  hideHover: 'auto',
				  resize: true
				});

			}	
			
			if ($('#graph_bar_group').length ){
			
				Morris.Bar({
				  element: 'graph_bar_group',
				  data: [
					{"period": "2016-10-01", "licensed": 807, "sorned": 660},
					{"period": "2016-09-30", "licensed": 1251, "sorned": 729},
					{"period": "2016-09-29", "licensed": 1769, "sorned": 1018},
					{"period": "2016-09-20", "licensed": 2246, "sorned": 1461},
					{"period": "2016-09-19", "licensed": 2657, "sorned": 1967},
					{"period": "2016-09-18", "licensed": 3148, "sorned": 2627},
					{"period": "2016-09-17", "licensed": 3471, "sorned": 3740},
					{"period": "2016-09-16", "licensed": 2871, "sorned": 2216},
					{"period": "2016-09-15", "licensed": 2401, "sorned": 1656},
					{"period": "2016-09-10", "licensed": 2115, "sorned": 1022}
				  ],
				  xkey: 'period',
				  barColors: ['#26B99A', '#34495E', '#ACADAC', '#3498DB'],
				  ykeys: ['licensed', 'sorned'],
				  labels: ['Licensed', 'SORN'],
				  hideHover: 'auto',
				  xLabelAngle: 60,
				  resize: true
				});

			}
			
			if ($('#graphx').length ){
			
				Morris.Bar({
				  element: 'graphx',
				  data: [
					{x: '2015 Q1', y: 2, z: 3, a: 4},
					{x: '2015 Q2', y: 3, z: 5, a: 6},
					{x: '2015 Q3', y: 4, z: 3, a: 2},
					{x: '2015 Q4', y: 2, z: 4, a: 5}
				  ],
				  xkey: 'x',
				  ykeys: ['y', 'z', 'a'],
				  barColors: ['#26B99A', '#34495E', '#ACADAC', '#3498DB'],
				  hideHover: 'auto',
				  labels: ['Y', 'Z', 'A'],
				  resize: true
				}).on('click', function (i, row) {
					console.log(i, row);
				});

			}
			
			if ($('#graph_area').length ){
			
				Morris.Area({
				  element: 'graph_area',
				  data: [
					{period: '2014 Q1', iphone: 2666, ipad: null, itouch: 2647},
					{period: '2014 Q2', iphone: 2778, ipad: 2294, itouch: 2441},
					{period: '2014 Q3', iphone: 4912, ipad: 1969, itouch: 2501},
					{period: '2014 Q4', iphone: 3767, ipad: 3597, itouch: 5689},
					{period: '2015 Q1', iphone: 6810, ipad: 1914, itouch: 2293},
					{period: '2015 Q2', iphone: 5670, ipad: 4293, itouch: 1881},
					{period: '2015 Q3', iphone: 4820, ipad: 3795, itouch: 1588},
					{period: '2015 Q4', iphone: 15073, ipad: 5967, itouch: 5175},
					{period: '2016 Q1', iphone: 10687, ipad: 4460, itouch: 2028},
					{period: '2016 Q2', iphone: 8432, ipad: 5713, itouch: 1791}
				  ],
				  xkey: 'period',
				  ykeys: ['iphone', 'ipad', 'itouch'],
				  lineColors: ['#26B99A', '#34495E', '#ACADAC', '#3498DB'],
				  labels: ['iPhone', 'iPad', 'iPod Touch'],
				  pointSize: 2,
				  hideHover: 'auto',
				  resize: true
				});

			}
			
			if ($('#graph_donut').length ){
			
				Morris.Donut({
				  element: 'graph_donut',
				  data: [
					{label: 'Jam', value: 25},
					{label: 'Frosted', value: 40},
					{label: 'Custard', value: 25},
					{label: 'Sugar', value: 10}
				  ],
				  colors: ['#26B99A', '#34495E', '#ACADAC', '#3498DB'],
				  formatter: function (y) {
					return y + "%";
				  },
				  resize: true
				});

			}
			
			if ($('#graph_line').length ){
			
				Morris.Line({
				  element: 'graph_line',
				  xkey: 'year',
				  ykeys: ['value'],
				  labels: ['Value'],
				  hideHover: 'auto',
				  lineColors: ['#26B99A', '#34495E', '#ACADAC', '#3498DB'],
				  data: [
					{year: '2012', value: 20},
					{year: '2013', value: 10},
					{year: '2014', value: 5},
					{year: '2015', value: 5},
					{year: '2016', value: 20}
				  ],
				  resize: true
				});

				$MENU_TOGGLE.on('click', function() {
				  $(window).resize();
				});
			
			}
			
}
function x() {
	if (document.title === "Repo - Ingenieria" || document.title === "Create - Ingenieria" || document.title === "Estado de OILs - Ingenieria" || document.title === "IPs - Ingenieria")
    $('.collapse-link').trigger("click");
}

$(function () {

    setTimeout(function () { $('.page-loader-wrapper').fadeOut(); }, 50);
});



function ReloadAfterDelay() {
	setTimeout(function () {
		window.location.reload(1);
	}, 2000);
}

function mOut(obj) {
	//obj.innerHTML = "Mouse Over Me"
	$('#Nav2').show();

	/* onmouseout="mOut(this)" onclick="$('#Nav2').hide();"*/
}

const labels_es_ES = {
	labelIdle: 'Arrastra y suelta tus archivos o <span class = "filepond--label-action"> Examinar <span>',
	labelInvalidField: "El campo contiene archivos inválidos",
	labelFileWaitingForSize: "Esperando tamaño",
	labelFileSizeNotAvailable: "Tamaño no disponible",
	labelFileLoading: "Cargando",
	labelFileLoadError: "Error durante la carga",
	labelFileProcessing: "Cargando",
	labelFileProcessingComplete: "Carga completa, Actualiza para ver tus archivos",
	labelFileProcessingAborted: "Carga cancelada",
	labelFileProcessingError: "Error durante la carga",
	labelFileProcessingRevertError: "Error durante la reversión",
	labelFileRemoveError: "Error durante la eliminación",
	labelTapToCancel: "toca para cancelar",
	labelTapToRetry: "tocar para volver a intentar",
	labelTapToUndo: "tocar para deshacer",
	labelButtonRemoveItem: "Eliminar",
	labelButtonAbortItemLoad: "Abortar",
	labelButtonRetryItemLoad: "Reintentar",
	labelButtonAbortItemProcessing: "Cancelar",
	labelButtonUndoItemProcessing: "Deshacer",
	labelButtonRetryItemProcessing: "Reintentar",
	labelButtonProcessItem: "Cargar",
	labelMaxFileSizeExceeded: "El archivo es demasiado grande",
	labelMaxFileSize: "El tamaño máximo del archivo es {filesize}",
	labelMaxTotalFileSizeExceeded: "Tamaño total máximo excedido",
	labelMaxTotalFileSize: "El tamaño total máximo del archivo es {filesize}",
	labelFileTypeNotAllowed: "Archivo de tipo no válido",
	fileValidateTypeLabelExpectedTypes: "Espera {allButLastType} o {lastType}",
	imageValidateSizeLabelFormatError: "Tipo de imagen no compatible",
	imageValidateSizeLabelImageSizeTooSmall: "La imagen es demasiado pequeña",
	imageValidateSizeLabelImageSizeTooBig: "La imagen es demasiado grande",
	imageValidateSizeLabelExpectedMinSize: "El tamaño mínimo es {minWidth} × {minHeight}",
	imageValidateSizeLabelExpectedMaxSize: "El tamaño máximo es {maxWidth} × {maxHeight}",
	imageValidateSizeLabelImageResolutionTooLow: "La resolución es demasiado baja",
	imageValidateSizeLabelImageResolutionTooHigh: "La resolución es demasiado alta",
	imageValidateSizeLabelExpectedMinResolution: "La resolución mínima es {minResolution}",
	imageValidateSizeLabelExpectedMaxResolution: "La resolución máxima es {maxResolution}",
};
//==========================================================================================================================

	$('.tabled').DataTable({
		"dom": 'lBfrtip',
		"lengthMenu": [[ 4,10, 25, 50, -1], ["4","10", "25", "50", "Todos"]],
		"buttons": ['copy', 'csv', 'print', 'pdf', 'excel'],
		"order": [[0, "desc"]],
		"scrollX": true,
		"language": {
			"sProcessing": "Procesando...",
			"sLengthMenu": "Mostrar _MENU_ registros",
			"sZeroRecords": "No se encontraron resultados",
			"sEmptyTable": "Ningun dato disponible en esta tabla =(",
			"sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
			"sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
			"sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
			"sInfoPostFix": "",
			"sSearch": "Buscar:",
			"sUrl": "",
			"sInfoThousands": ",",
			"sLoadingRecords": "Cargando...",
			"oPaginate": {
				"sFirst": "Primero",
				"sLast": "ultimo",
				"sNext": "Siguiente",
				"sPrevious": "Anterior"
			},
			"oAria": {
				"sSortAscending": ": Activar para ordenar la columna de manera ascendente",
				"sSortDescending": ": Activar para ordenar la columna de manera descendente"
			},
			"buttons": {
				"copy": "Copiar",
				"colvis": "Visibilidad"
			}
		}

	});


// Panel toolbox
$(document).ready(function () {

	$('#dtc1').datetimepicker({ format: 'DD/MM/YYYY HH:mm:ss' });
	$('#dtc2').datetimepicker({ format: 'DD/MM/YYYY HH:mm:ss' });
	$('#dpk1').datetimepicker({
		format: 'DD/MM/YYYY'
	});
	$('#dpk1').datetimepicker({
		format: 'DD/MM/YYYY'
	});
	$('#dpk2').datetimepicker({
		format: 'DD/MM/YYYY'
	});
	$('#dpk3').datetimepicker({
		format: 'DD/MM/YYYY'
	});
	$('#dpk4').datetimepicker({
		format: 'DD/MM/YYYY'
	});
	$('#dpk5').datetimepicker({
		format: 'DD/MM/YYYY'
	});
	$('#dpk6').datetimepicker({
		format: 'DD/MM/YYYY'
	});
	$('#dpk7').datetimepicker({
		format: 'DD/MM/YYYY'
	});
	$('#dpk8').datetimepicker({
		format: 'DD/MM/YYYY'
	});

	$('#dtp1').datetimepicker({
		format: 'MM/YYYY'
	});
	$('#dtp2').datetimepicker({
		format: 'DD/MM/YYYY'
	});
	$('#dtpi').datetimepicker({
		format: 'DD/MM/YYYY'
	});
	$('#dtpf').datetimepicker({
		format: 'DD/MM/YYYY'
	});
	$('#dtpa').datetimepicker({
		format: 'YYYY'
	});
	$('#dtpm').datetimepicker({
		format: 'MM'
	});
	$('#dtpd').datetimepicker({
		format: 'DD'
	});
	$('#dtpaf').datetimepicker({
		format: 'YYYY'
	});
	$('#dtpmf').datetimepicker({
		format: 'MM'
	});
	$('#dtpdf').datetimepicker({
		format: 'DD'
	});

    init_morris_charts();
 
    $('.collapse-link').on('click', function () {
        var $BOX_PANEL = $(this).closest('.x_panel'),
            $ICON = $(this).find('i'),
            $BOX_CONTENT = $BOX_PANEL.find('.x_content');

        // fix for some div with hardcoded fix class
        if ($BOX_PANEL.attr('style')) {
            $BOX_CONTENT.slideToggle(200, function () {
                $BOX_PANEL.removeAttr('style');
            });
        } else {
            $BOX_CONTENT.slideToggle(200);
            $BOX_PANEL.css('height', 'auto');
        }

        $ICON.toggleClass('fa-chevron-up fa-chevron-down');
    });

    $('.close-link').click(function () {
        var $BOX_PANEL = $(this).closest('.x_panel');

        $BOX_PANEL.remove();
    });
    x();

    jQuery("time.timeago").timeago();
});
// /Panel toolbox

