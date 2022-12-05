using System;
using Foundation;
using ObjCRuntime;

namespace ImageCaptureCore {
	partial class ICCameraDevice {
		public delegate void DidReadDataDelegate (NSData data, ICCameraFile file, NSError error);
		
		public void RequestReadDataFromFile (ICCameraFile file, long offset, long length, DidReadDataDelegate callback)
		{
			var actionObject = new DidReadDataFromFileAction (callback);
			RequestReadDataFromFile (file, offset, length, actionObject, new Selector("didReadData:fromFile:error:contextInfo:"), IntPtr.Zero);
    	}
		
		class DidReadDataFromFileAction : NSObject {
			DidReadDataDelegate Callback;
			public const string CallbackSelector = "didReadData:fromFile:error:contextInfo:";

			public DidReadDataFromFileAction (DidReadDataDelegate callback)
			{
				Callback = callback;
				IsDirectBinding = false;
			}

			[Export (CallbackSelector)]
			// having same name as delegate ??
			void DidReadDataDelegate (NSData data, ICCameraFile file, NSError error, IntPtr contextInfo)
			{
				Callback (data, file, error);
			}
		}

		// // RequestSendPtpCommand
		// public void RequestSendPtpCommand (NSData command, NSData outData, DidSendPtpCommandDelegate callback)
		// {
		// 	var actionObject = new DidSendPtpCommandAction (callback);
		// 	RequestSendPtpCommand (command, outData, actionObject, Selector.GetHandle (DidSendPtpCommandAction.CallbackSelector), IntPtr.Zero);
    	// }
		
		// class DidSendPtpCommandAction : NSObject {
		// 	DidSendPtpCommand Callback;
		// 	public const string CallbackSelector = "didSendPTPCommand:inData:response:error:contextInfo";

		// 	public SendPtpCommandAction (SendPtpCommandAction callback)
		// 	{
		// 		Callback = callback;
		// 		IsDirectBinding = false;
		// 	}

		// 	[Export (CallbackSelector)]
		// 	void DidSendPtpCommandDelegate (NSData command, NSData inData, NSData response, NSError error, IntPtr contextInfo)
		// 	{
		// 		Callback (command, inData, response, error);
		// 	}
		// }

		// // RequestDownloadFileCommand
		// public void RequestDownloadFile (ICCameraFile file, NSDictionary<NSString, NSObject> options, DidDownloadFileDelegate callback)
		// {
		// 	var actionObject = new DidDownloadFile (callback);
		// 	RequestDownloadFile (file, options, actionObject, Selector.GetHandle (DidDownloadFileAction.CallbackSelector), IntPtr.Zero);
    	// }
		
		// class DidDownloadFileAction : NSObject {
		// 	DidDownloadFile Callback;
		// 	public const string CallbackSelector = "didDownloadFile:error:options:contextInfo:";

		// 	public DownloadFileAction (DownloadFileAction callback)
		// 	{
		// 		Callback = callback;
		// 		IsDirectBinding = false;
		// 	}

		// 	[Export (CallbackSelector)]
		// 	void DidDownloadFile (NSData command, NSError error, NSDictionary<NSString, NSObject> options, IntPtr contextInfo)
		// 	{
		// 		Callback (command, error, options);
		// 	}
		// }


	}

}
