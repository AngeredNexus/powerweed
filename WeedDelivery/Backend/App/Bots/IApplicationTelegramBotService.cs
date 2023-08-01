using WeedDatabase.Domain.App;
using WeedDelivery.Backend.Models.Api.Bots;
using WeedDelivery.Backend.Models.Api.Common;

namespace WeedDelivery.Backend.App.Bots;

public interface IApplicationTelegramBotService
{
    /// <summary>
    /// 1. Получить список ИД операторов(в т.ч фильтуя по мете из заказа)
    /// 2. Сформировать ответ(в соответствии с языком)
    /// 3. Обратиться к некому единному сервису телеги, который позволит по типу бота вытащить его экземпляр и передать туда сообщение по ИД юзера
    /// Блокирует: фабрика ботов не автоматизирована, 
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    Task NotifyAboutOrder(BotOrderNotification order, TelegramCoockie userData);
}