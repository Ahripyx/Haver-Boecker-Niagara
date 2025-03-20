// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.




document.addEventListener("DOMContentLoaded", (e) => {
    //The ones that are not in using are for later on the project please dont remove
    const page = document.body.dataset.page;
    const btnCreate = document.getElementById('btnCreate');
    const btnDelete = document.getElementById('btnClose');
    const btnEdit = document.getElementById('btnEdit');
    const notification = document.querySelector('.notifications')
    const toastSucess = document.querySelector('.toast.success');
    const toastError = document.querySelector('.toast.error');
    const toastWarning = document.querySelector('.toast.warning');
    const toastInfo = document.querySelector('.toast.info');
    const logoutIcon = document.querySelector('#logout-icon');
    const btnClose = document.querySelector('#btnDelete');
    const btnContinue = document.querySelector('#btnContinue');
    const btnFitler = document.querySelector('#btnFilter');

    
    /*//////////////////////////////////////
    Toast Notifications 
    *///////////////////////////////////////
    //Remove the toast by clicking X
    const removeToast = (t) => {
        if (t) {
            t.style.display = 'none';
            setTimeout(() => t.remove(), 500);
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
        console.log('Create button exists')
        btnCreate.addEventListener("click", () => {
            sessionStorage.setItem("showToast", "true");
            const inputs = document.querySelectorAll('input[required]');
            let error = false;
            inputs.forEach(i => {
                if (!i.value.trim()) {
                    error = true;
                }
            })
            if (error) {
                if (toastError) {
                    sessionStorage.removeItem("showToast");
                    toastError.style.display = 'flex';
                    setTimeout(() => {
                        toastError.style.display = "none";
                    }, 5000);
                }
            }
            else {
                sessionStorage.setItem("showToast, true")
            }
        });
    }
    if (btnEdit) {
        console.log('Edit button exists')
        btnEdit.addEventListener("click", () => {
            sessionStorage.setItem("showToast", "true");
            const inputs = document.querySelectorAll('input[required]');
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
            } else {
                sessionStorage.setItem("showToast, true")
            }
        });
    }
    if (btnDelete) {
        console.log('Delete button exists')
        btnDelete.addEventListener("click", () => {
            sessionStorage.setItem("showToast", "true");
        });
    }

    if (btnFitler) {
        console.log('btnFilter exists');
        btnFitler.addEventListener('click', () => {
            sessionStorage.setItem("showInfoToast", "true");
        })
    }
    window.addEventListener('load', () => {
        if (sessionStorage.getItem('showInfoToast') === 'true') {
            if (toastInfo) {
                toastInfo.style.display = 'flex';
                sessionStorage.removeItem('showInfoToast');
                setTimeout(() => {
                    toastInfo.style.display = 'none';
                    sessionStorage.removeItem('showInfoToast');
                }, 5000)
            }
        }
    })
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
        console.log('logout button exists')
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
        console.log('close button exists')
        const PopupMainConteiner = document.querySelector('.popup-close-conteiner');
        const btnCloseCancel = document.querySelector('#btnCloseCancel');
       
        btnClose.addEventListener('click', e => {
            e.preventDefault();
            PopupMainConteiner.style.display = 'flex';
        });
        if (btnCloseCancel) {
            console.log('btnCancel exists')
            btnCloseCancel.addEventListener('click', () => {
                console.log('btn close clicked')
                PopupMainConteiner.style.display = 'none';
            });
        }
        if (btnContinue) {
            btnContinue.addEventListener('click', () => {
                // window.location.href = btnClose.href;
                if (toastWarning) {
                    sessionStorage.setItem("showToast", "true");
                }
                document.getElementsByTagName('form')[0].submit();
                
            })
        }
    }
})