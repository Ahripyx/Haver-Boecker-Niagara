// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.




document.addEventListener("DOMContentLoaded", (e) => {
    //The ones that are not in using are for later on the project please dont remove
    const page = document.body.dataset.page;
    const btnCreate = document.getElementById('btnCreate');
    const btnDelete = document.getElementById('btnDelete');
    const btnEdit = document.getElementById('btnEdit');
    const notification = document.querySelector('.notifications')
    const toastSucess = document.querySelector('.toast.success');
    const toastError = document.querySelector('.toast.error');
    const toastWarning = document.querySelector('.toast.warning');
    const toastInfo = document.querySelector('.toast.info');
    const logoutIcon = document.querySelector('#logout-icon');
    const btnClose = document.querySelector('#btnClose');
    const btnContinue = document.querySelector('#btnContinue');

    /*//////////////////////////////////////
    Toast Notifications 
    *///////////////////////////////////////
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

    //Popup functionality 

    if (logoutIcon) {
        console.log('button there')
        const PopupMainConteiner = document.querySelector('.popup-conteiner');
        const btnLogOut = document.querySelector('#btnLogOut');
        const btnCancel = document.querySelector('#btnCancel');
        logoutIcon.addEventListener('click', e => {
            e.preventDefault();
            PopupMainConteiner.style.display = 'flex';
        });
        if (btnCancel) {
            btnCancel.addEventListener('click', () => {
                PopupMainConteiner.style.display = 'none';
            });
        }
        if (btnLogOut) { 
            btnLogOut.addEventListener('click', () => {
                const logoutUrl = btnLogOut.getAttribute("data-logut-url");
                window.location.href = logoutUrl;
            });
        }
    }
    
    if (btnClose) {
        console.log('Btn close there')
        const PopupMainConteiner = document.querySelector('.popup-close-conteiner');
        const btnCloseCancel = document.querySelector('#btnCloseCancel');
       
        btnClose.addEventListener('click', e => {
            e.preventDefault();
            PopupMainConteiner.style.display = 'flex';
        });
        if (btnCloseCancel) {
            console.log('btnCancel there')
            btnCloseCancel.addEventListener('click', () => {
                console.log('btn close clicked')
                PopupMainConteiner.style.display = 'none';
            });
        }
        if (btnContinue) {
            btnContinue.addEventListener('click', () => {
                window.location.href = btnClose.href;
                if (toastWarning) {
                    sessionStorage.setItem("showToast", "true");
                }
            })
        }
    }
})