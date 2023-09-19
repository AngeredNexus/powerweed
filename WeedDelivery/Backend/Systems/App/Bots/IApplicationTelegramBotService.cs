using WeedDatabase.Domain.Common;
using WeedDelivery.Backend.Models.Api.Bots;

namespace WeedDelivery.Backend.Systems.App.Bots;

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
    Task NotifyAboutOrder(BotOrderNotification order, SmokiUser userData);
}