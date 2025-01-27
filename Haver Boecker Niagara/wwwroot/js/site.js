// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


/*//////////////////////////////////////
    Toast Notifications 
*///////////////////////////////////////

document.addEventListener("DOMContentLoaded", (e) => {
    const page = document.body.dataset.page;
    const btnCreate = document.getElementById('btnCreate');
    const btnDelete = document.getElementById('btnDelete');
    const btnEdit = document.getElementById('btnEdit');
    const notification = document.querySelector('.notifications')
    const toastSucess = document.querySelector('.toast.success');
    const toastError = document.querySelector('.toast.error');
    const toastWarning = document.querySelector('.toast.warning');
    const toastInfo = document.querySelector('.toast.info');
    
    //Remove the toast by clicking X
    const removeToast = (t) => {
        if (t) {
            t.style.display = 'none';
            setTimeout(() => toast.remove(), 500);
        }
    }
    document.querySelectorAll('.close-toast').forEach(i => {
        i.addEventListener('click', (e) => {
            const toast = e.target.closest('.toast'); 
            removeToast(toast);
        });
    });
    //Create the toasts
    if (btnCreate) {
        console.log('Btn exist')
        btnCreate.addEventListener("click", () => {
            sessionStorage.setItem("showToast", "true");
            const inputs = document.querySelectorAll('input');
            let error = false;
            inputs.forEach(i => {
                if (!i.value.trim()) {
                    error = true;
                }
            })
            if (error) {
                if (toastError) {
                    toastError.style.display = 'flex';
                    setTimeout(() => {
                        toastError.style.display = "none";
                    }, 5000);
                }
            } 
        });
    }
    if (btnEdit) {
        console.log('Btn exist')
        btnEdit.addEventListener("click", () => {
            sessionStorage.setItem("showToast", "true"); 
            const inputs = document.querySelectorAll('input');
            let error = false;
            inputs.forEach(i => {
                if (!i.value.trim()) {
                    error = true;
                }
            })
            if (error) {
                if (toastError) {
                    toastError.style.display = 'flex';
                    setTimeout(() => {
                        toastError.style.display = "none";
                    }, 5000);
                }
            } 
        });
    }
    if (btnDelete) {
        console.log('Btn exist')
        btnDelete.addEventListener("click", () => {
            sessionStorage.setItem("showToast", "true");
        });
    }

    const showToast = sessionStorage.getItem("showToast");
    
    if (showToast === "true") {
        const toast = document.querySelector('.toast.success');
        if (toast) {
            toast.style.display = "flex";

            sessionStorage.removeItem("showToast");

           
            setTimeout(() => {
                toast.style.display = "none";
            }, 5000);
        }
    }
   
})