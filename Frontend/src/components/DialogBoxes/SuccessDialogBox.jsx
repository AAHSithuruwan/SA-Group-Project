const SuccessDialogBox = (options) => {

    return window.Swal.fire({
        position: "top",  
        title: options.title || 'Success!',
        text: options.text || '',
        icon: 'success',
        confirmButtonText: 'OK',
        width: 600,
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
  
  export default SuccessDialogBox;