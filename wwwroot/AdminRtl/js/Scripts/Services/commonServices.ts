
const  commonServices = {
    setCustomHtml5InputsRequiredTitle() {
        document.addEventListener("DOMContentLoaded", function () {
            var elements = document.getElementsByTagName("INPUT");
            for (var i = 0; i < elements.length; i++) {
                (elements[i] as any).oninvalid = function (e:any) {
                    e.target.setCustomValidity("");
                    if (!e.target.validity.valid) {
                        e.target.setCustomValidity("This field cannot be left blank");
                    }
                };
                (elements[i] as any).oninput = function (e:any) {
                    e.target.setCustomValidity("");
                };
            }
        })
    }
}
 