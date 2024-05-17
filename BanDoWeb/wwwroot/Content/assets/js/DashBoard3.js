/*"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/dashboardHub").build();


$(function () {
	connection.start().then(function () {
		alert('Connected to dashboardHub');

		InvokeOderheader();
	}).catch(function (err) {
		return console.error(err.toString());
	});
});

function InvokeOderheader() {
	connection.invoke("SendOderHeader").catch(function (err) {
		return console.error(err.toString());
	});
}

connection.on("ReceivedOderHeader", function (oderheader) {
	BindProductsToGrid(oderheader);
});
function BindProductsToGrid(oderheader) {
	$('#tblTable tbody').empty();

	var tr;
	$.each(oderheader, function (index, oderheader) {
		tr = $('<tr/>');
		tr.append(`<td><a href="pages/examples/invoice.html">OR9842</a></td>`);
		tr.append(`<td>${(oderheader.lastName)} </td>`);
		tr.append(`<td><span class="badge badge-success">${(oderheader.oderStatus)}`);
		tr.append(`<td><div class="sparkbar">${(oderheader.oderStatus)}</div></td>`);
		$('#tblTable').append(tr);
	});
}*/

var areaChartCanvas = $('#areaChart').get(0).getContext('2d')
var barChartCanvas = $('#barChart').get(0).getContext('2d')
var stackedBarChartCanvas = $('#stackedBarChart').get(0).getContext('2d')
var lineChart = $('#lineChart').get(0).getContext('2d')
$(() => {


	Statistics()
	StatisticsUser()
	StatisticsUserNumberOfVs()
	StockChart()
	var connection = new signalR.HubConnectionBuilder().withUrl("/signalsServer").build();

	connection.on("LoadStatis", function () {
		Statistics()
		StatisticsUser()
		StatisticsUserNumberOfVs()
		StockChart()
	});
})
function Statistics() {
	$.ajax({
		url: '/Admin/homeadmin/statistics',
		type: 'GET',
		data: {},
		success: function (res) {
			var result = res
			var arrDoanhThu = [];
			var arrDate = [];
			var arrProfit = [];
			$.each(result.data, function (i, item,) {
				var strDate = item.month + '-' + item.year
				var strProfit = (parseInt(item.total) - parseInt(item.profit))
				arrProfit.push(strProfit)
				arrDate.push(strDate);
				arrDoanhThu.push(item.total);

			})
			var areaChartData = {
				labels: arrDate,
				datasets: [
					{
						label: 'Doanh thu',
						backgroundColor: 'rgba(60,141,188,0.9)',
						borderColor: 'rgba(60,141,188,0.8)',
						pointRadius: false,
						pointColor: '#3b8bba',
						pointStrokeColor: 'rgba(60,141,188,1)',
						pointHighlightFill: '#fff',
						pointHighlightStroke: 'rgba(60,141,188,1)',
						data: arrDoanhThu
					},
					{
						label: 'Lợi nhuận',
						backgroundColor: 'rgba(210, 214, 222, 1)',
						borderColor: 'rgba(210, 214, 222, 1)',
						pointRadius: false,
						pointColor: 'rgba(210, 214, 222, 1)',
						pointStrokeColor: '#c1c7d1',
						pointHighlightFill: '#fff',
						pointHighlightStroke: 'rgba(220,220,220,1)',
						data: arrProfit 
					},
				]
			}

			var areaChartOptions = {
				maintainAspectRatio: false,
				responsive: true,
				legend: {
					display: false
				},
				scales: {
					xAxes: [{
						gridLines: {
							display: false,
						}
					}],
					yAxes: [{
						gridLines: {
							display: false,
						}
					}]
				}
			}

			new Chart(areaChartCanvas, {
				type: 'line',
				data: areaChartData,
				options: areaChartOptions
			})

			//
			var barChartData = $.extend(true, {}, areaChartData)
			var temp0 = areaChartData.datasets[0]
			var temp1 = areaChartData.datasets[1]
			barChartData.datasets[0] = temp1
			barChartData.datasets[1] = temp0

			var barChartOptions = {
				responsive: true,
				maintainAspectRatio: false,
				datasetFill: false
			}

			new Chart(barChartCanvas, {
				type: 'bar',
				data: barChartData,
				options: barChartOptions
			})
		}
	});
}
function StatisticsUser() {
	$.ajax({
		url: '/Admin/homeadmin/StatisticsUser',
		type: 'GET',
		data: {},
		success: function (res) {
			var result = res
			var arrUser = [];
			var arrDate = []
			$.each(result.data, function (i, item,) {
				var strDate = item.month + '-' + item.year
				arrDate.push(strDate);
				arrUser.push(item.count);

			})
			var areaChartData = {
				labels: arrDate,
				datasets: [
					{
						label: 'Tài khoản',
						backgroundColor: 'rgba(60,141,188,0.9)',
						borderColor: 'rgba(60,141,188,0.8)',
						pointRadius: false,
						pointColor: '#3b8bba',
						pointStrokeColor: 'rgba(60,141,188,1)',
						pointHighlightFill: '#fff',
						pointHighlightStroke: 'rgba(60,141,188,1)',
						data: arrUser
					},
				]
			}
			var stackedBarChartData = $.extend(true, {}, areaChartData)
			var stackedBarChartOptions = {
				responsive: true,
				maintainAspectRatio: false,
				scales: {
					xAxes: [{
						stacked: true,
					}],
					yAxes: [{
						stacked: true
					}]
				}
			}

			new Chart(stackedBarChartCanvas, {
				type: 'bar',
				data: stackedBarChartData,
				options: stackedBarChartOptions
			})
		}
	})
}
function StatisticsUserNumberOfVs() {
	$.ajax({
		url: '/Admin/homeadmin/StatisticsUserNumberOfVs',
		type: 'GET',
		data: {},
		success: function (res) {
			var result = res
			var arrNumber = [];
			var arrDate = []
			$.each(result.data, function (i, item,) {
				var strDate = item.month + '-' + item.year
				arrDate.push(strDate);
				arrNumber.push(item.number);
			})
			var data = {
				labels: arrDate,
				datasets: [{
					label: 'Truy cập',
					backgroundColor: 'rgba(75, 192, 192, 0.2)',
					borderColor: 'rgba(75, 192, 192, 1)',
					borderWidth: 1,
					data: arrNumber
				}]
			};
			var options = {
				scales: {
					y: {
						beginAtZero: true
					}
				}
			};
			new Chart(lineChart, {
				type: 'bar',
				data: data,
				options: options
			});
		}
	})
}

function StockChart() {
	var ctx = document.getElementById('stockChart').getContext('2d');
	$.ajax({
		url: '/Admin/homeadmin/StockChart',
		type: 'GET',
		data: {},
		success: function (res) {
			var result = res
			var arrCategory = [];
			var arrSum = []
			$.each(result.data, function (i, item,) {
				arrCategory.push(item.name);
				arrSum.push(item.sumw);

			})
			var myChart = new Chart(ctx, {
				type: 'bar',
				data: {
					labels: arrCategory,
					datasets: [{
						label: 'Tổng kho hàng',
						data: arrSum,
						backgroundColor: 'rgba(54, 162, 235, 0.5)',
						borderColor: 'rgba(54, 162, 235, 1)',
						borderWidth: 1
					}]
				},
				options: {
					scales: {
						y: {
							beginAtZero: true
						}
					}
				}
			})
		}
		})
}


