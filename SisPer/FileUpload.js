function Comprobar() {
    var fileInput = document.getElementById("fileInput");

    var message = "";
    var validFileExtensions = new Array("jpg", "png", "gif");
    var fileExtension = "";
    var size = 0;

    if ('files' in fileInput) {
        if (fileInput.files.length == 0) {
            message = "Please browse for one or more files.";
        } else {
            var file = fileInput.files[0];
            if ('size' in file) {
                size = file.size;
            }
            else {
                size = file.fileSize;
            }
            if ('name' in file) {
                fileExtension = file.name.substr(file.name.length - 3, 3);
            }
            else {
                fileExtension = file.fileName.substr(file.fileName.length - 3, 3);
            }

            var flag = false;
            // loop over the valid file extensions to compare them with uploaded file
            for (var index = 0; index < validFileExtensions.length; index++) {
                if (fileExtension.toLowerCase() == validFileExtensions[index].toString().toLowerCase()) {
                    flag = true;
                }
            }

            if (flag == false) {
                alert("Archivo no permitido");
                return false;
            }
            else {
                if (size / 1024 > 500) {
                    alert("El tamaño del achivo supera los 500 KB permitidos, modifíquelo o seleccione otro.");
                    return false;
                }
                else {
                    return true;
                }
            }
        }
    }
    else {
        if (fileInput.value == "") {
            alert("No ha seleccionado ningún archivo.")
            return false;
        }
        else {
            alert("Su navegador no soporta esta funcionalidad.")
            return false;
        }
    }
}
