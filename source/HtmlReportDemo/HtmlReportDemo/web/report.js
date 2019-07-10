
function zoomIn(){
    var zoom = document.body.style.zoom ? parseFloat(document.body.style.zoom) : 1;
    if (zoom < 4.0) zoom = zoom + 0.1;
    document.body.style.zoom = zoom;
}
function zoomOut(){
    var zoom = document.body.style.zoom ? parseFloat(document.body.style.zoom) : 1;
    if (zoom > 0.2) zoom = zoom - 0.1;
    document.body.style.zoom = zoom;
}
