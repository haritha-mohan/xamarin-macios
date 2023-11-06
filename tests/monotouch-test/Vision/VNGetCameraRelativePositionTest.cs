// Unit test for Vision.GetCameraRelativePosition

#if !__WATCHOS__

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

using CoreGraphics;
using ImageIO;
using Foundation;
using Vision;

#if NET
using System.Numerics;
#else
using OpenTK;
#endif

namespace MonoTouchFixtures.Vision
{

    [TestFixture]
    [Preserve(AllMembers = true)]
    public class VNGetCameraRelativePositionTest
    {

		// NSUrl imageUrl;
		// [SetUp]
		// public void Setup() => TestRuntime.AssertXcodeVersion(15, 0);
		[SetUp]
		public void SetUp()
		{
			TestRuntime.AssertXcodeVersion(15, 0);

			// imageUrl = NSBundle.MainBundle.GetUrlForResource("portrait", "heic");
			// imageData = NSData.FromUrl(imageUrl);
		}

		// [TearDown]
		// public void TearDown()
		// {
		// 	imageUrl?.Dispose();
		// 	// imageData?.Dispose();
		// }

		[Test]
		public void GetCameraRelativePositionTest()
		{
			// NSBundle.BundleForClass(new Class(typeof(VNGetCameraRelativePositionTest))).GetUrlForResource("portrait", "png");
			var requestHandler = new VNImageRequestHandler(NSBundle.MainBundle.GetUrlForResource("full_body", "png"), new NSDictionary());
			var request = new VNDetectHumanBodyPose3DRequest();
			var didPerform = requestHandler.Perform(new VNRequest[] { request }, out NSError error);
			// Assert.That(didPerform, Is.True, "VNImageRequestHandler.Perform should return true.");
			Assert.Null (error, $"VNImageRequestHandler.Perform should not return an error {error}");
			var observation = request.Results[0];
			Assert.That (1, Is.EqualTo(request.Results.Length), "there is more than 1 result present..");

			var recognizedPoints = observation.GetRecognizedPoints (VNHumanBodyPose3DObservationJointsGroupName.Head, out NSError error22);
			Assert.
			Assert.NotNull (recognizedPoints, "RecognizedPoints should not return null.");
			Assert.Null(error22, $"Recognized points should not return an error {error22}");

			var jointNames = new List<VNHumanBodyPose3DObservationJointName> { 
				VNHumanBodyPose3DObservationJointName.Root, 
			VNHumanBodyPose3DObservationJointName.RightHip, 
			VNHumanBodyPose3DObservationJointName.RightKnee,  
			VNHumanBodyPose3DObservationJointName.RightAnkle, 
			VNHumanBodyPose3DObservationJointName.RightShoulder, 
			VNHumanBodyPose3DObservationJointName.CenterShoulder, 
			VNHumanBodyPose3DObservationJointName.CenterHead,
			VNHumanBodyPose3DObservationJointName.TopHead, };

			foreach (var i in jointNames) {
				var position = observation.GetCameraRelativePosition (out Vector4 modelPositionOut, i, out NSError error2);
				Assert.NotNull(modelPositionOut, "GetCameraRelativePosition should not return null.");
				Assert.False (position, "GetCameraRelativePosition should return false, but actually it was able to identify something!");
				Assert.That (modelPositionOut, Is.EqualTo(Vector4.Zero), "VNVector3DGetCameraRelativePosition is not empty!");
			}
			
			// var position = observation.GetCameraRelativePosition(out Vector4 modelPositionOut, VNHumanBodyPose3DObservationJointName.CenterHead, out NSError error2);
			// Assert.NotNull(modelPositionOut, "GetCameraRelativePosition should not return null.");
			// Assert.True (position, "GetCameraRelativePosition should return true.");
			// Assert.Null (error2, $"GetCameraRelativePosition should not return an error {error2}");
			// Assert.That (modelPositionOut, Is.Not.EqualTo(Vector4.Zero), "VNVector3DGetCameraRelativePosition is empty");

			// var position = observation.GetCameraRelativePosition(out Vector4 modelPositionOut, VNHumanBodyPose3DObservationJointName., out NSError error2);


			// var hi = new VNDetectHumanBodyPose3DRequest();
			// var res = hi.Results[0];
			// var val = res.GetCameraRelativePosition(out Vector4 modelPositionOut, VNHumanBodyPose3DObservationJointName.RightElbow, out NSError error);
			// Assert.NotNull(modelPositionOut, "GetCameraRelativePosition should not return null.");
			// Assert.That(modelPositionOut, Is.Not.EqualTo(Vector4.Zero), "VNVector3DGetCameraRelativePosition is not empty");
			// var cameraPosition = VNUtils.GetCameraRelativePosition(new Vector3(1, 2, 3), new Vector3(4, 5, 6), new Vector3(7, 8, 9));
			// Assert.That(cameraPosition, Is.Not.EqualTo(Vector3.Zero), "VNVector3DGetCameraRelativePosition is not empty");
		}
	}
}
#endif
