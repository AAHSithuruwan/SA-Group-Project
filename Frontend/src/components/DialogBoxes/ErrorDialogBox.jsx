const ErrorDialogBox = (options) => {

    return window.Swal.fire({
        position: "top",  
        title: options.title || 'Error!',
        text: options.text || '',
        icon: 'error',
        confirmButtonText: 'OK',
        width: 600,
        confirmButtonColor : '#FF0000',
        allowOutsideClick: false,
        allowEscapeKey: false,
        // Use the Bulma theme
        customClass: {
            popup: 'bulma',
        },
        

    }).then((result) => {
        // Check if the "OK" button was clicked
        if (result.isConfirmed) {
            if (options.onConfirm) {
                // Execute the function passed in as the onConfirm callback
                options.onConfirm();
            }
        }
    });
  };
  
  export default ErrorDialogBox;