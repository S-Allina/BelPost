var search = document.querySelector(".search");
search.addEventListener('input', function () { // Изменено на 'input'
    document.querySelectorAll(".name").forEach(u => {
        if (u.textContent.toLowerCase().includes(search.value.toLowerCase()) && search.value != "" && u.closest(".PostageStampCard").style.border != "4px solid rgb(31, 189, 31)") { // Приведение к нижнему регистру

            u.closest(".PostageStampCard").style.border = "4px solid black";
            console.log(u.closest(".PostageStampCard"));
            console.log("yes")
        } else {
            //if (u.closest(".PostageStampCard").style.border != "4px solid rgb(31, 189, 31)") {
                u.closest(".PostageStampCard").style.border = "1px solid gray"; // Сброс границы
            //}
        }
    })
})