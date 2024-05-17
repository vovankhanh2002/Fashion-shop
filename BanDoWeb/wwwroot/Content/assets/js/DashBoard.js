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


$(() => {
	
	
	getCountViewProduct()
	getCountApplication()
	LoadCountOderDetails()
	LoadDataOderHeader();
	getCountOrderDetail()
	var connection = new signalR.HubConnectionBuilder().withUrl("/signalsServer").build();

	connection.on("LoadOrderHeader", function () {
		LoadDataOderHeader();
		LoadDataOderDetail()
		LoadCountOderDetails()
		getCountApplication()
		getCountViewProduct()
		getCountOrderDetail()
	});
	var connectionChat = new signalR.HubConnectionBuilder().withUrl("/chathub").build();
	connectionChat.on("ReceiveMessage", function (sender, message) {
		$('#receiverInput').val(sender)
		
		if (sender != "khanh12345@gmail.com") {
			MessageSender(sender, message)
		} else {
			MessageReceiver(sender, message)
		}

		var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
		var encodedMsg = sender + " says " + msg;
		var li = document.createElement("li");
		li.textContent = encodedMsg;
		document.getElementById("message").appendChild(li);
		
	});
	connectionChat.start().then(function () {
		document.getElementById("sendButton").disabled = false;
	}).catch(function (err) {
		return console.error(err.toString());
	});
	
	$('#btnViewOder').click(function () {
		LoadDataOderDetail()
	})
	$('#btnPlaceOder').click(function () {
		LoadDataOderHeader()
	})

	
	$('#sendButton').click(function (event) {
		var sender = document.getElementById("senderInput").value;
		var receiver = document.getElementById("receiverInput").value;
		var message = document.getElementById("messageInput").value;
		console.log(sender)
		if (message != "") {
			if (receiver != "") {
				if (sender == "khanh12345@gmail.com") {
					MessageReceiver(sender, message)
				}
				else {
					var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
					var encodedMsg = sender + " says " + msg;
					var li = document.createElement("li");
					li.textContent = encodedMsg;
					document.getElementById("message").appendChild(li);

				}
				connectionChat.invoke("SendMessageToGroup", sender, receiver, message)
				$('#messageInput').val("")
			}
			else {
				connectionChat.invoke("SendMessage", sender, message).catch(function (err) {
					console.log(senMessage(user, message))
					return console.error(err.toString());
				});
			}
			event.preventDefault();
		}
		
    })
	
})
function LoadDataOderHeader() {
	var tr = ''
	$.ajax({
		url: '/Admin/OderHeader/getOrderHeader',
		method: 'GET',
		success: (result) => {
			$.each(result, function (index, oderheader) {
				tr += '<tr/>'
				tr += `<td>${(oderheader.firstName)}</td>`
				tr += `<td>${(oderheader.oderDate)} </td>`
				tr += `<td><span class="badge badge-success">${(oderheader.oderStatus)}`
				tr += `<td><div class="sparkbar">${(oderheader.oderTotal)}</div></td>`
				tr += '</tr>'

			});
			$('#tblTable').html(tr);

		}
	});
	$.ajax({
		url: '/Admin/OderHeader/getCountOrderDetail',
		method: 'GET',
		success: (result) => {
			$('#tblTable').html(tr);
		}
    })
}
function LoadDataOderDetail() {
	var tr = ''
	$.ajax({
		url: '/Admin/OderHeader/getOrderDetail',
		method: 'GET',
		success: (result) => {
			
			$.each(result, function (index, oderdetail) {
					tr += '<tr/>'
						tr += `<td>${(oderdetail.oderHeaderId)}</td>`
						tr += `<td>${(oderdetail.productId)} </td>`
						tr += `<td><span class="badge badge-success">${(oderdetail.count)}`
						tr += `<td><div class="sparkbar">${(oderdetail.price)}</div></td>`
						tr += `<td><div class="sparkbar">${(oderdetail.tittle)}</div></td>`
						tr += `<td><div class="sparkbar">${(oderdetail.color)}</div></td>`
						tr += `<td><div class="sparkbar">${(oderdetail.size)}</div></td>`
						tr += '</tr>'
                 
				
			});
			$('#tblTable').html(tr);

		}
	})
}
function LoadCountOderDetails() {
	$.ajax({
		url: '/Admin/OderDetails/getCountOrderDetail',
		method: 'GET',
		success: (result) => {
			$('#CountSales').html(result);
		}
	})
}
function getCountApplication() {
	$.ajax({
		url: '/Admin/ApplicationUser/getCountApplication',
		method: 'GET',
		success: (result) => {
			$('#countApplication').html(result);
		}
	})
}
function getCountViewProduct() {
	$.ajax({
		url: '/Admin/Product/getCoutViewsProd',
		method: 'GET',
		success: (result) => {
			$('#countViews').html(result);
		}
	})
}
function getCountOrderDetail() {
	$.ajax({
		url: '/Admin/OderDetails/getCountOrderDetail',
		method: 'GET',
		success: (result) => {
			$('#countDetail').html(result);
		}
	})
}
function MessageReceiver(sender, message) {
	var now = new Date(Date.now());
	var formatted = now.getHours() + ":" + now.getMinutes();
	var tr = `<div class="direct-chat-infos clearfix">`
	tr += `<span class="direct-chat-name float-right">${sender}</span>`
	tr += `<span class="direct-chat-timestamp float-left">${formatted}</span>`
	tr += `</div>`
	tr += `<div class="direct-chat-text float-right">${message}</div>`
	$('#messageAdmin').append(tr);
}
function MessageSender(sender, message) {
	console.log("koo")
	var now = new Date(Date.now());
	var formatted = now.getHours() + ":" + now.getMinutes();
	var tr = `<div class="direct-chat-infos clearfix">`
	tr += `<span class="direct-chat-name float-left">${sender}</span>`
	tr += `<span class="direct-chat-timestamp float-right">${formatted}</span>`
	tr += `</div>`
	tr += `<div class="direct-chat-text float-left">${message}</div>`
	$('#messageSender').append(tr);
}





