using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BoxOwlWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BoxOwlWeb.ViewModels {
    public class ClientViewModel {

        [DisplayName("Имя")]
        public string ClientName { get; set; }
        [DisplayName("Фамилия")]
        public string ClientSurname { get; set; }
        [DisplayName("Телефон")]
        public string ClientPhone { get; set; }

        [Remote(action: "CheckEmail", controller: "Account", ErrorMessage = "E-mail уже используется")]
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        [DisplayName("E-mail")]
        public string ClientEmail { get; set; }
        [DisplayName("Пароль")]
        public string ClientPassword { get; set; }

        [Compare("ClientPassword", ErrorMessage = "Пароли не совпадают")]
        [DisplayName("Подтверждение пароля")]
        public string ClientConfirmPassword { get; set; }
    }
}