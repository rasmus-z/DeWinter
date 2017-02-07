﻿using System;
using System.Collections.Generic;
using Core;

namespace DeWinter
{
	public static class DeWinterApp
	{
		public static T RegisterModel<T>() where T:IModel, new()
		{
			return App.Service<ModelSvc>().Register<T>();
		}

		public static T GetModel<T>() where T:IModel
		{
			return App.Service<ModelSvc>().GetModel<T>();
		}

		public static void SendCommand<T>() where T:ICommand, new()
		{
			App.Service<CommandSvc>().Execute<T>();
		}

		public static void SendCommand<T, U>(U data) where T:ICommand<U>, new()
		{
			App.Service<CommandSvc>().Execute<T, U>(data);
		}

		public static void SendMessage(string messageID)
		{
			App.Service<MessageSvc>().Send(messageID);
		}

		public static void SendMessage<T>(string messageID, T data)
		{
			App.Service<MessageSvc>().Send<T>(messageID, data);
		}

		public static void SendMessage<T>(T data)
		{
			App.Service<MessageSvc>().Send<T>(data);
		}

		public static void Subscribe(string messageID, Action callback)
		{
			App.Service<MessageSvc>().Subscribe(messageID, callback);
		}

		public static void Subscribe<T>(string messageID, Action<T> callback)
		{
			App.Service<MessageSvc>().Subscribe<T>(messageID, callback);
		}

		public static void Subscribe<T>(Action<T> callback)
		{
			App.Service<MessageSvc>().Subscribe<T>(callback);
		}
	}
}