function connect_local (e) {
	alert('TODO:  Create SSH tunnels')
}

function open_ssh(uri) {
	var w = Titanium.UI.createWindow({
		url: "http://demos.anyterm.org/shellinabox_nano/",
		height: 500,
		width: 600
	});
	w.open();
}
$(document).ready(function()
{
	var window = Titanium.UI.currentWindow;

	window.setHeight(800); 
	window.setWidth(1278); 
});

Titanium.API.addEventListener(Titanium.EXIT, function(event) {
	if (!confirm("Are you sure you want to exit?")) {
		event.preventDefault();
	}
})

