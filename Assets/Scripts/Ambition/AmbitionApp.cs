﻿using UnityEngine;

using System;
using System.Collections.Generic;
using Dialog;
using UFlow;
using Core;

namespace Ambition
{
	public static class AmbitionApp
	{
		public static T RegisterModel<T>() where T:IModel, new()
		{
			return App.Service<ModelSvc>().Register<T>();
		}

		public static void UnregisterModel<T>() where T:IModel
		{
			App.Service<ModelSvc>().Unregister<T>();
		}

		public static T GetModel<T>() where T:IModel
		{
			return App.Service<ModelSvc>().GetModel<T>();
		}

		/// COMMANDS
		public static void RegisterCommand<C>(string messageID) where C:ICommand, new()
		{
			App.Service<CommandSvc>().Register<C>(messageID);
		}

		public static void RegisterCommand<C, T>() where C:ICommand<T>, new()
		{
			App.Service<CommandSvc>().Register<C, T>();
		}

		public static void RegisterCommand<C, T>(string messageID) where C:ICommand<T>, new()
		{
			App.Service<CommandSvc>().Register<C, T>(messageID);
		}

		public static void Execute<C>() where C:ICommand, new()
		{
			App.Service<CommandSvc>().Execute<C>();
		}

		public static void Execute<C, T>(T value) where C:ICommand<T>, new()
		{
			App.Service<CommandSvc>().Execute<C, T>(value);
		}

		public static void UnregisterCommand<C>(string messageID) where C:ICommand
		{
			App.Service<CommandSvc>().Unregister<C>(messageID);
		}

		public static void UnregisterCommand<C, T>(string messageID) where C:ICommand<T>
		{
			App.Service<CommandSvc>().Unregister<C, T>(messageID);
		}

		public static void UnregisterCommand<C, T>() where C:ICommand<T>
		{
			App.Service<CommandSvc>().Unregister<C, T>();
		}

		/// MESSAGES
		public static void SendMessage(string messageID)
		{
			App.Service<MessageSvc>().Send(messageID);
		}

		public static void SendMessage<T>(string messageID, T msg)
		{
			App.Service<MessageSvc>().Send<T>(messageID, msg);
		}

		public static void SendMessage<T>(T msg)
		{
			App.Service<MessageSvc>().Send<T>(msg);
		}

		public static void Subscribe<T>(Action<T> callback)
		{
			App.Service<MessageSvc>().Subscribe<T>(callback);
		}

		public static void Subscribe(string messageID, Action callback)
		{
			App.Service<MessageSvc>().Subscribe(messageID, callback);
		}

		public static void Subscribe<T>(string messageID, Action<T> callback)
		{
			App.Service<MessageSvc>().Subscribe(messageID, callback);
		}

		public static void Unsubscribe(string message, Action callback)
		{
			App.Service<MessageSvc>().Unsubscribe(message, callback);
		}

		public static void Unsubscribe<T>(string message, Action<T> callback)
		{
			App.Service<MessageSvc>().Unsubscribe<T>(message, callback);
		}

		public static void Unsubscribe<T>(Action<T> callback)
		{
			App.Service<MessageSvc>().Unsubscribe<T>(callback);
		}

		public static GameObject OpenDialog(string DialogID)
		{
			return App.Service<DialogSvc>().Open(DialogID);
		}

		public static GameObject OpenDialog<T>(string DialogID, T Data)
		{
			return App.Service<DialogSvc>().Open<T>(DialogID, Data);
		}

		public static GameObject OpenMessageDialog(string dialogID, Dictionary<string, string> substitutions)
		{
			MessageDialogVO vo = new MessageDialogVO();
			vo.Title = GetModel<LocalizationModel>().GetString(dialogID + DialogConsts.TITLE, substitutions);
			vo.Body = GetModel<LocalizationModel>().GetString(dialogID + DialogConsts.BODY, substitutions);
			vo.Button = GetModel<LocalizationModel>().GetString(dialogID + DialogConsts.OK, substitutions);
			if (vo.Button == null) vo.Button = GetModel<LocalizationModel>().GetString(DialogConsts.DEFAULT_CONFIRM);
			return OpenDialog<MessageDialogVO>(DialogConsts.MESSAGE, vo);
		}

		public static GameObject OpenMessageDialog(string dialogID)
		{
			MessageDialogVO vo = new MessageDialogVO();
			vo.Title = GetModel<LocalizationModel>().GetString(dialogID + DialogConsts.TITLE);
			vo.Body = GetModel<LocalizationModel>().GetString(dialogID + DialogConsts.BODY);
			vo.Button = GetModel<LocalizationModel>().GetString(dialogID + DialogConsts.OK);
			if (vo.Button == null) vo.Button = GetModel<LocalizationModel>().GetString(DialogConsts.DEFAULT_CONFIRM);
			return OpenDialog<MessageDialogVO>(DialogConsts.MESSAGE, vo);
		}

		public static void CloseDialog(string dialogID)
		{
			App.Service<DialogSvc>().Close(dialogID);
		}

		public static void CloseDialog(GameObject dialog)
		{
			App.Service<DialogSvc>().Close(dialog);
		}

		// Easy shorthand for creating a request to modify a model value
		public static void AdjustValue<T>(string ID, T value)
		{
			RequestAdjustValueVO<T> vo = new RequestAdjustValueVO<T>(ID, value);
			App.Service<MessageSvc>().Send<RequestAdjustValueVO<T>>(vo);
		}

		public static void RegisterState<C>(string stateID, string machineID) where C : UState, new()
		{
			App.Service<UFlowSvc>().RegisterState<C>(stateID, machineID);
		}
	}
}