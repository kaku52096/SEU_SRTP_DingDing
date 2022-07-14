﻿// ControlData.cs
// Tore Knabe
// Copyright 2020 tore.knabe@gmail.com

using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

// ReSharper disable CommentTypo
// ReSharper disable PossibleNullReferenceException
// ReSharper disable UnusedParameter.Global

namespace RealisticEyeMovements
{
	[Serializable]
	public partial class ControlData
	{
	
		#region fields

			#region Store setup to see whether Look up etc needs resaving

				bool wasSetupStored;
				
				EyeControl eyeControlStored;
				EyelidControl eyelidControlStored;
				EyelidBoneMode eyelidBoneModeStored;
				Transform leftEyeStored;
				Transform rightEyeStored;
				List<Transform> upperLeftEyelidBonesStored = new List<Transform>();
				List<Transform> upperRightEyelidBonesStored = new List<Transform>();
				List<Transform> lowerLeftEyelidBonesStored = new List<Transform>();
				List<Transform> lowerRightEyelidBonesStored = new List<Transform>();

				bool isEyeBoneDefaultSetStored;
				bool isEyeBallDefaultSetStored;
				bool isEyelidBonesDefaultSetStored;
				bool isEyelidBlendshapeDefaultSetStored;
				bool isEyelidBonesClosedSetStored;
				bool isEyelidBlendshapeClosedSetStored;
				bool isEyeBoneLookDownSetStored;
				bool isEyeBallLookDownSetStored;
				bool isEyelidBonesLookDownSetStored;
				bool isEyelidBlendshapeLookDownSetStored;
				bool isEyeBoneLookUpSetStored;
				bool isEyeBallLookUpSetStored;
				bool isEyelidBonesLookUpSetStored;
				bool isEyelidBlendshapeLookUpSetStored;

			#endregion

			#region Eyes

				public enum EyeControl
				{
					None,
					MecanimEyeBones,
					SelectedObjects
				}

				public EyeControl eyeControl = EyeControl.None;

				public Transform leftEye;
				public Transform rightEye;

				public float maxEyeUpBoneAngle = 20;
				public float maxEyeDownBoneAngle = 20;

				public float maxEyeUpEyeballAngle = 20;
				public float maxEyeDownEyeballAngle = 20;

				public bool isEyeBallDefaultSet;
				public bool isEyeBoneDefaultSet;
				public bool isEyeBallLookUpSet;
				public bool isEyeBoneLookUpSet;
				public bool isEyeBallLookDownSet;
				public bool isEyeBoneLookDownSet;

				[SerializeField] EyeRotationLimiter leftBoneEyeRotationLimiter = new EyeRotationLimiter();
				[SerializeField] EyeRotationLimiter rightBoneEyeRotationLimiter = new EyeRotationLimiter();
				[SerializeField] EyeRotationLimiter leftEyeballEyeRotationLimiter = new EyeRotationLimiter();
				[SerializeField] EyeRotationLimiter rightEyeballEyeRotationLimiter = new EyeRotationLimiter();

			#endregion


			#region Eyelids

				public enum EyelidControl
				{
					None,
					Bones,
					Blendshapes
				}

				public EyelidControl eyelidControl = EyelidControl.None;

				public enum EyelidBoneMode
				{
					RotationAndPosition,
					Rotation,
					Position
				}

				public EyelidBoneMode eyelidBoneMode = EyelidBoneMode.RotationAndPosition;

				#region Eyelid Bones

					// These are obsolete and will be removed in a future update
					[SerializeField] Transform upperEyeLidLeft;
					[SerializeField] Transform upperEyeLidRight;
					[SerializeField] Transform lowerEyeLidLeft;
					[SerializeField] Transform lowerEyeLidRight;

					public List<Transform> upperLeftEyelidBones = new List<Transform>();
					public List<Transform> upperRightEyelidBones = new List<Transform>();
					public List<Transform> lowerLeftEyelidBones = new List<Transform>();
					public List<Transform> lowerRightEyelidBones = new List<Transform>();

					public bool isEyelidBonesDefaultSet;
					public bool isEyelidBonesClosedSet;
					public bool isEyelidBonesLookUpSet;
					public bool isEyelidBonesLookDownSet;

					// These are obsolete and will be removed in a future update
					[SerializeField] EyelidRotationLimiter upperLeftLimiter = new EyelidRotationLimiter();
					[SerializeField] EyelidRotationLimiter upperRightLimiter = new EyelidRotationLimiter();
					[SerializeField] EyelidRotationLimiter lowerLeftLimiter = new EyelidRotationLimiter();
					[SerializeField] EyelidRotationLimiter lowerRightLimiter = new EyelidRotationLimiter();

					[SerializeField] List<EyelidRotationLimiter> upperLeftLimiters = new List<EyelidRotationLimiter>();
					[SerializeField] List<EyelidRotationLimiter> upperRightLimiters = new List<EyelidRotationLimiter>();
					[SerializeField] List<EyelidRotationLimiter> lowerLeftLimiters = new List<EyelidRotationLimiter>();
					[SerializeField] List<EyelidRotationLimiter> lowerRightLimiters = new List<EyelidRotationLimiter>();

					public float eyeWidenOrSquint;
					
					bool wereEyelidsRestoredToDefaultSinceLastUpdate;

				#endregion


				#region Eyelid Blendshapes

					[SerializeField] EyelidPositionBlendshape[] blendshapesForBlinking = new EyelidPositionBlendshape[0];
					[SerializeField] EyelidPositionBlendshape[] blendshapesForLookingUp = new EyelidPositionBlendshape[0];
					[SerializeField] EyelidPositionBlendshape[] blendshapesForLookingDown = new EyelidPositionBlendshape[0];

					[Serializable]
					public class BlendshapesConfig
					{
						public SkinnedMeshRenderer skinnedMeshRenderer;
						public int blendShapeCount;
						public float[] blendshapeWeights;
					}

					[SerializeField] BlendshapesConfig[] blendshapesConfigs = new BlendshapesConfig[0];

					public bool isEyelidBlendshapeDefaultSet;
					public bool isEyelidBlendshapeClosedSet;
					public bool isEyelidBlendshapeLookUpSet;
					public bool isEyelidBlendshapeLookDownSet;

				#endregion

			#endregion

		#endregion


		public bool CanImport(ControlDataForSerialization import, Transform startXform, Transform usedHeadXform)
		{
			bool canImport = true;

			Transform boneStartXform = import.arePathsRelativeToHead ? usedHeadXform : startXform;

			if (import.eyeControl == EyeControl.SelectedObjects)
			{
				canImport = Utils.CanGetTransformFromPath(boneStartXform, import.leftEyePath, "left eye") &&
				            Utils.CanGetTransformFromPath(boneStartXform, import.rightEyePath, "right eye") &&
				            leftEyeballEyeRotationLimiter.CanImport(import.leftEyeballEyeRotationLimiter, boneStartXform,
					            "left eye object") &&
				            rightEyeballEyeRotationLimiter.CanImport(import.rightEyeballEyeRotationLimiter, boneStartXform,
					            "right eye object");
			}
			else if (import.eyeControl == EyeControl.MecanimEyeBones)
			{
				Animator animator = startXform.GetComponentInChildren<Animator>();
				if (animator == null)
				{
					Debug.LogError("Cannot import, eye control set to Mecanim eyes, but no animator found.",
						startXform.gameObject);
					return false;
				}

				leftEye = animator.GetBoneTransform(HumanBodyBones.LeftEye);
				if (leftEye == null)
				{
					Debug.LogError("Cannot import, eye control set to Mecanim eyes, but left eye not found.",
						startXform.gameObject);
					return false;
				}

				rightEye = animator.GetBoneTransform(HumanBodyBones.RightEye);
				if (rightEye == null)
				{
					Debug.LogError("Cannot import, eye control set to Mecanim eyes, but right eye not found.",
						startXform.gameObject);
					return false;
				}
			}

			foreach (string upperLeftEyelidBonePath in import.upperLeftEyelidBonePaths)
				canImport = canImport &&
				            Utils.CanGetTransformFromPath(boneStartXform, upperLeftEyelidBonePath, "upper left eyelid");
			foreach (string upperLeftEyelidBonePath in import.upperRightEyelidBonePaths)
				canImport = canImport &&
				            Utils.CanGetTransformFromPath(boneStartXform, upperLeftEyelidBonePath, "upper left eyelid");
			foreach (string upperLeftEyelidBonePath in import.lowerLeftEyelidBonePaths)
				canImport = canImport &&
				            Utils.CanGetTransformFromPath(boneStartXform, upperLeftEyelidBonePath, "upper left eyelid");
			foreach (string upperLeftEyelidBonePath in import.lowerRightEyelidBonePaths)
				canImport = canImport &&
				            Utils.CanGetTransformFromPath(boneStartXform, upperLeftEyelidBonePath, "upper left eyelid");

			foreach (var limiterFromImport in import.upperLeftLimiters)
				canImport = canImport &&
				            EyelidRotationLimiter.CanImport(limiterFromImport, boneStartXform, "upper left eyelids");
			foreach (var limiterFromImport in import.upperRightLimiters)
				canImport = canImport &&
				            EyelidRotationLimiter.CanImport(limiterFromImport, boneStartXform, "upper right eyelids");
			foreach (var limiterFromImport in import.lowerLeftLimiters)
				canImport = canImport &&
				            EyelidRotationLimiter.CanImport(limiterFromImport, boneStartXform, "lower left eyelids");
			foreach (var limiterFromImport in import.upperRightLimiters)
				canImport = canImport &&
				            EyelidRotationLimiter.CanImport(limiterFromImport, boneStartXform, "upper right eyelids");

			if (false == canImport)
				return false;

			if (import.blendshapesForBlinking != null)
				foreach (EyelidPositionBlendshapeForExport blendShapeImport in import.blendshapesForBlinking)
					if (false == EyelidPositionBlendshape.CanImport(blendShapeImport, startXform))
					{
						Debug.LogError("Cannot import, blendshape not found: " + blendShapeImport.name,
							startXform.gameObject);
						return false;
					}

			if (import.blendshapesForLookingUp != null)
				foreach (EyelidPositionBlendshapeForExport blendShapeImport in import.blendshapesForLookingUp)
					if (false == EyelidPositionBlendshape.CanImport(blendShapeImport, startXform))
					{
						Debug.LogError("Cannot import, blendshape not found: " + blendShapeImport.name,
							startXform.gameObject);
						return false;
					}

			if (import.blendshapesForLookingDown != null)
				foreach (EyelidPositionBlendshapeForExport blendShapeImport in import.blendshapesForLookingDown)
					if (false == EyelidPositionBlendshape.CanImport(blendShapeImport, startXform))
					{
						Debug.LogError("Cannot import, blendshape not found: " + blendShapeImport.name,
							startXform.gameObject);
						return false;
					}

			return true;
		}


		public float ClampLeftVertEyeAngle(float angle)
		{
			if (eyeControl == EyeControl.MecanimEyeBones)
				return leftBoneEyeRotationLimiter.ClampAngle(angle);

			if (eyeControl == EyeControl.SelectedObjects)
				return leftEyeballEyeRotationLimiter.ClampAngle(angle);

			return angle;
		}


		public float ClampRightVertEyeAngle(float angle)
		{
			if (eyeControl == EyeControl.MecanimEyeBones)
				return rightBoneEyeRotationLimiter.ClampAngle(angle);

			if (eyeControl == EyeControl.SelectedObjects)
				return rightEyeballEyeRotationLimiter.ClampAngle(angle);

			return angle;
		}

		
		public void ConvertLegacyIfNecessary()
		{
			//*** We used to have only one Transform per eyelid, now we have a list.
			{
				if (upperEyeLidLeft != null && upperLeftEyelidBones.Count == 0)
				{
					upperLeftEyelidBones.Add(upperEyeLidLeft);
					upperEyeLidLeft = null;

					upperLeftLimiters.Clear();
					upperLeftLimiters.Add(upperLeftLimiter);
					upperLeftLimiter = null;
				}

				if (upperEyeLidRight != null && upperRightEyelidBones.Count == 0)
				{
					upperRightEyelidBones.Add(upperEyeLidRight);
					upperEyeLidRight = null;

					upperRightLimiters.Clear();
					upperRightLimiters.Add(upperRightLimiter);
					upperRightLimiter = null;
				}

				if (lowerEyeLidLeft != null && lowerLeftEyelidBones.Count == 0)
				{
					lowerLeftEyelidBones.Add(lowerEyeLidLeft);
					lowerEyeLidLeft = null;

					lowerLeftLimiters.Clear();
					lowerLeftLimiters.Add(lowerLeftLimiter);
					lowerLeftLimiter = null;
				}

				if (lowerEyeLidRight != null && lowerRightEyelidBones.Count == 0)
				{
					lowerRightEyelidBones.Add(lowerEyeLidRight);
					lowerEyeLidRight = null;

					lowerRightLimiters.Clear();
					lowerRightLimiters.Add(lowerRightLimiter);
					lowerRightLimiter = null;
				}
			}
		}
		

		void CreateNewBlendshapeConfigs(Transform startXform)
		{
			SkinnedMeshRenderer[] skinnedMeshRenderers = startXform.GetComponentsInChildren<SkinnedMeshRenderer>();
			blendshapesConfigs = new BlendshapesConfig[skinnedMeshRenderers.Length];
			for (int i = 0; i < skinnedMeshRenderers.Length; i++)
			{
				BlendshapesConfig blendshapesConfig = new BlendshapesConfig
					{skinnedMeshRenderer = skinnedMeshRenderers[i]};
				blendshapesConfig.blendShapeCount = blendshapesConfig.skinnedMeshRenderer.sharedMesh.blendShapeCount;
				blendshapesConfig.blendshapeWeights = new float[blendshapesConfig.blendShapeCount];
				for (int j = 0; j < blendshapesConfig.blendShapeCount; j++)
					blendshapesConfig.blendshapeWeights[j] =
						blendshapesConfig.skinnedMeshRenderer.GetBlendShapeWeight(j);
				blendshapesConfigs[i] = blendshapesConfig;
			}
		}


		public ControlDataForSerialization GetExport(Transform startXform, Transform usedHeadXform)
		{
			bool arePathsRelativeToHead =	usedHeadXform != null &&
															(leftEye == null || leftEye.IsChildOf(usedHeadXform)) &&
															(rightEye == null || rightEye.IsChildOf(usedHeadXform)) &&
															(leftBoneEyeRotationLimiter.transform == null ||
															leftBoneEyeRotationLimiter.transform.IsChildOf(usedHeadXform)) &&
															(rightBoneEyeRotationLimiter.transform == null ||
															rightBoneEyeRotationLimiter.transform.IsChildOf(usedHeadXform)) &&
															(leftEyeballEyeRotationLimiter.transform == null ||
															leftEyeballEyeRotationLimiter.transform.IsChildOf(usedHeadXform)) &&
															(rightEyeballEyeRotationLimiter.transform == null ||
															rightEyeballEyeRotationLimiter.transform.IsChildOf(usedHeadXform)) &&
															Utils.AreTransformsChildrenOf(upperLeftEyelidBones, usedHeadXform) &&
															Utils.AreTransformsChildrenOf(upperRightEyelidBones, usedHeadXform) &&
															Utils.AreTransformsChildrenOf(lowerLeftEyelidBones, usedHeadXform) &&
															Utils.AreTransformsChildrenOf(lowerRightEyelidBones, usedHeadXform);

			Transform boneStartXform = arePathsRelativeToHead ? usedHeadXform : startXform;

			ControlDataForSerialization serialization = new ControlDataForSerialization
			{
				eyeControl = eyeControl,
				eyelidBoneMode = eyelidBoneMode,
				leftEyePath = Utils.GetPathForTransform(boneStartXform, leftEye),
				rightEyePath = Utils.GetPathForTransform(boneStartXform, rightEye),
				maxEyeUpBoneAngle = maxEyeUpBoneAngle,
				maxEyeDownBoneAngle = maxEyeDownBoneAngle,
				maxEyeUpEyeballAngle = maxEyeUpEyeballAngle,
				maxEyeDownEyeballAngle = maxEyeDownEyeballAngle,
				isEyeBallDefaultSet = isEyeBallDefaultSet,
				isEyeBoneDefaultSet = isEyeBoneDefaultSet,
				isEyeBallLookUpSet = isEyeBallLookUpSet,
				isEyeBoneLookUpSet = isEyeBoneLookUpSet,
				isEyeBallLookDownSet = isEyeBallLookDownSet,
				isEyeBoneLookDownSet = isEyeBoneLookDownSet,
				leftBoneEyeRotationLimiter = leftBoneEyeRotationLimiter.GetExport(boneStartXform),
				rightBoneEyeRotationLimiter = rightBoneEyeRotationLimiter.GetExport(boneStartXform),
				leftEyeballEyeRotationLimiter = leftEyeballEyeRotationLimiter.GetExport(boneStartXform),
				rightEyeballEyeRotationLimiter = rightEyeballEyeRotationLimiter.GetExport(boneStartXform),
				eyelidControl = eyelidControl,
				isEyelidBonesDefaultSet = isEyelidBonesDefaultSet,
				isEyelidBonesClosedSet = isEyelidBonesClosedSet,
				isEyelidBonesLookUpSet = isEyelidBonesLookUpSet,
				isEyelidBonesLookDownSet = isEyelidBonesLookDownSet,
				eyeWidenOrSquint = eyeWidenOrSquint,
				isEyelidBlendshapeDefaultSet = isEyelidBlendshapeDefaultSet,
				isEyelidBlendshapeClosedSet = isEyelidBlendshapeClosedSet,
				isEyelidBlendshapeLookUpSet = isEyelidBlendshapeLookUpSet,
				isEyelidBlendshapeLookDownSet = isEyelidBlendshapeLookDownSet,
				blendshapesForBlinking = new EyelidPositionBlendshapeForExport[blendshapesForBlinking.Length],
				arePathsRelativeToHead =	arePathsRelativeToHead
			};


			
			foreach (var limiter in upperLeftLimiters)
				serialization.upperLeftLimiters.Add(limiter.GetExport(boneStartXform));
			foreach (var limiter in upperRightLimiters)
				serialization.upperRightLimiters.Add(limiter.GetExport(boneStartXform));
			foreach (var limiter in lowerLeftLimiters)
				serialization.lowerLeftLimiters.Add(limiter.GetExport(boneStartXform));
			foreach (var limiter in lowerRightLimiters)
				serialization.lowerRightLimiters.Add(limiter.GetExport(boneStartXform));

			//*** Eyelid bones
			{
				foreach (Transform upperLeftEyelidBone in upperLeftEyelidBones)
					serialization.upperLeftEyelidBonePaths.Add(Utils.GetPathForTransform(boneStartXform, upperLeftEyelidBone));
				foreach (Transform upperRightEyelidBone in upperRightEyelidBones)
					serialization.upperRightEyelidBonePaths.Add(Utils.GetPathForTransform(boneStartXform, upperRightEyelidBone));
				foreach (Transform lowerLeftEyelidBone in lowerLeftEyelidBones)
					serialization.lowerLeftEyelidBonePaths.Add(Utils.GetPathForTransform(boneStartXform, lowerLeftEyelidBone));
				foreach (Transform lowerRightEyelidBone in lowerRightEyelidBones)
					serialization.lowerRightEyelidBonePaths.Add(Utils.GetPathForTransform(boneStartXform, lowerRightEyelidBone));
			}
			
			for (int i = 0; i < blendshapesForBlinking.Length; i++)
				serialization.blendshapesForBlinking[i] = blendshapesForBlinking[i].GetExport(startXform);

			serialization.blendshapesForLookingUp = new EyelidPositionBlendshapeForExport[blendshapesForLookingUp.Length];
			for (int i = 0; i < blendshapesForLookingUp.Length; i++)
				serialization.blendshapesForLookingUp[i] = blendshapesForLookingUp[i].GetExport(startXform);

			serialization.blendshapesForLookingDown = new EyelidPositionBlendshapeForExport[blendshapesForLookingDown.Length];
			for (int i = 0; i < blendshapesForLookingDown.Length; i++)
				serialization.blendshapesForLookingDown[i] = blendshapesForLookingDown[i].GetExport(startXform);

			return serialization;
		}


		public void Import(ControlDataForSerialization import, Transform startXform, Transform usedHeadXform)
		{
			Transform boneStartXform = import.arePathsRelativeToHead ? usedHeadXform : startXform;

			eyeControl = import.eyeControl;
			eyelidBoneMode = import.eyelidBoneMode;
			if (import.eyeControl == EyeControl.SelectedObjects)
			{
				leftEye = Utils.GetTransformFromPath(boneStartXform, import.leftEyePath);
				rightEye = Utils.GetTransformFromPath(boneStartXform, import.rightEyePath);
			}
			else if (import.eyeControl == EyeControl.MecanimEyeBones)
			{
				Animator animator = startXform.GetComponentInChildren<Animator>();
				leftEye = animator.GetBoneTransform(HumanBodyBones.LeftEye);
				rightEye = animator.GetBoneTransform(HumanBodyBones.RightEye);
			}

			maxEyeUpBoneAngle = import.maxEyeUpBoneAngle;
			maxEyeDownBoneAngle = import.maxEyeDownBoneAngle;
			maxEyeUpEyeballAngle = import.maxEyeUpEyeballAngle;
			maxEyeDownEyeballAngle = import.maxEyeDownEyeballAngle;
			isEyeBallDefaultSet = import.isEyeBallDefaultSet;
			isEyeBoneDefaultSet = import.isEyeBoneDefaultSet;
			isEyeBallLookUpSet = import.isEyeBallLookUpSet;
			isEyeBoneLookUpSet = import.isEyeBoneLookUpSet;
			isEyeBallLookDownSet = import.isEyeBallLookDownSet;
			isEyeBoneLookDownSet = import.isEyeBoneLookDownSet;
			eyelidControl = import.eyelidControl;
			isEyelidBonesDefaultSet = import.isEyelidBonesDefaultSet;
			isEyelidBonesClosedSet = import.isEyelidBonesClosedSet;
			isEyelidBonesLookUpSet = import.isEyelidBonesLookUpSet;
			isEyelidBonesLookDownSet = import.isEyelidBonesLookDownSet;
			eyeWidenOrSquint = import.eyeWidenOrSquint;

			upperLeftEyelidBones.Clear();
			upperRightEyelidBones.Clear();
			lowerLeftEyelidBones.Clear();
			lowerRightEyelidBones.Clear();
			foreach (string path in import.upperLeftEyelidBonePaths)
				upperLeftEyelidBones.Add(Utils.GetTransformFromPath(boneStartXform, path));
			foreach (string path in import.upperRightEyelidBonePaths)
				upperRightEyelidBones.Add(Utils.GetTransformFromPath(boneStartXform, path));
			foreach (string path in import.lowerLeftEyelidBonePaths)
				lowerLeftEyelidBones.Add(Utils.GetTransformFromPath(boneStartXform, path));
			foreach (string path in import.lowerRightEyelidBonePaths)
				lowerRightEyelidBones.Add(Utils.GetTransformFromPath(boneStartXform, path));

			isEyelidBlendshapeDefaultSet = import.isEyelidBlendshapeDefaultSet;
			isEyelidBlendshapeClosedSet = import.isEyelidBlendshapeClosedSet;
			isEyelidBlendshapeLookUpSet = import.isEyelidBlendshapeLookUpSet;
			isEyelidBlendshapeLookDownSet = import.isEyelidBlendshapeLookDownSet;

			leftBoneEyeRotationLimiter.Import(import.leftBoneEyeRotationLimiter, leftEye);
			rightBoneEyeRotationLimiter.Import(import.rightBoneEyeRotationLimiter, rightEye);
			leftEyeballEyeRotationLimiter.Import(import.leftEyeballEyeRotationLimiter,
				Utils.GetTransformFromPath(boneStartXform, import.leftEyeballEyeRotationLimiter.transformPath));
			rightEyeballEyeRotationLimiter.Import(import.rightEyeballEyeRotationLimiter,
				Utils.GetTransformFromPath(boneStartXform, import.rightEyeballEyeRotationLimiter.transformPath));

			upperLeftLimiters.Clear();
			upperRightLimiters.Clear();
			lowerLeftLimiters.Clear();
			lowerRightLimiters.Clear();
			foreach (var limiterFromImport in import.upperLeftLimiters)
				upperLeftLimiters.Add(EyelidRotationLimiter.Import(limiterFromImport, boneStartXform));
			foreach (var limiterFromImport in import.upperRightLimiters)
				upperRightLimiters.Add(EyelidRotationLimiter.Import(limiterFromImport, boneStartXform));
			foreach (var limiterFromImport in import.lowerLeftLimiters)
				lowerLeftLimiters.Add(EyelidRotationLimiter.Import(limiterFromImport, boneStartXform));
			foreach (var limiterFromImport in import.lowerRightLimiters)
				lowerRightLimiters.Add(EyelidRotationLimiter.Import(limiterFromImport, boneStartXform));

			if (import.blendshapesForBlinking != null)
			{
				blendshapesForBlinking = new EyelidPositionBlendshape[import.blendshapesForBlinking.Length];
				for (int i = 0; i < import.blendshapesForBlinking.Length; i++)
				{
					EyelidPositionBlendshape eyelidPositionBlendshape = new EyelidPositionBlendshape();
					eyelidPositionBlendshape.Import(import.blendshapesForBlinking[i], startXform);
					blendshapesForBlinking[i] = eyelidPositionBlendshape;
				}
			}

			if (import.blendshapesForLookingUp != null)
			{
				blendshapesForLookingUp = new EyelidPositionBlendshape[import.blendshapesForLookingUp.Length];
				for (int i = 0; i < import.blendshapesForLookingUp.Length; i++)
				{
					EyelidPositionBlendshape eyelidPositionBlendshape = new EyelidPositionBlendshape();
					eyelidPositionBlendshape.Import(import.blendshapesForLookingUp[i], startXform);
					blendshapesForLookingUp[i] = eyelidPositionBlendshape;
				}
			}

			if (import.blendshapesForLookingDown != null)
			{
				blendshapesForLookingDown = new EyelidPositionBlendshape[import.blendshapesForLookingDown.Length];
				for (int i = 0; i < import.blendshapesForLookingDown.Length; i++)
				{
					EyelidPositionBlendshape eyelidPositionBlendshape = new EyelidPositionBlendshape();
					eyelidPositionBlendshape.Import(import.blendshapesForLookingDown[i], startXform);
					blendshapesForLookingDown[i] = eyelidPositionBlendshape;
				}
			}

			ConvertLegacyIfNecessary();

			blendshapesConfigs = null;

			StoreSetup();
		}


		public void Initialize(Transform startXform)
		{
			if (eyelidControl == EyelidControl.Blendshapes)
			{
				//*** For each blinking blendshape save in "isUsedInEalierConfig" whether it is used in lookingUp or lookingDown
				//		(If for example the character is looking up and therefore it has modified the blendshape "EyelidUpperUp", then if blinking is set to relative mode,
				//		when blinking, the blinking delta value will be applied to this modified value if isusedInEalierConfig is set, otherwise to the default value)
				{
					foreach (EyelidPositionBlendshape blendShapeForBlinking in blendshapesForBlinking)
					{
						blendShapeForBlinking.isUsedInEalierConfig = false;

						foreach (EyelidPositionBlendshape blendShapeForLookingUp in blendshapesForLookingUp)
							if (blendShapeForBlinking.skinnedMeshRenderer ==
							    blendShapeForLookingUp.skinnedMeshRenderer &&
							    blendShapeForBlinking.index == blendShapeForLookingUp.index)
							{
								blendShapeForBlinking.isUsedInEalierConfig = true;
								break;
							}

						if (false == blendShapeForBlinking.isUsedInEalierConfig)
							foreach (EyelidPositionBlendshape blendShapeForLookingDown in blendshapesForLookingDown)
								if (blendShapeForBlinking.skinnedMeshRenderer ==
								    blendShapeForLookingDown.skinnedMeshRenderer &&
								    blendShapeForBlinking.index == blendShapeForLookingDown.index)
								{
									blendShapeForBlinking.isUsedInEalierConfig = true;
									break;
								}
					}
				}
			}
		}


		void LerpBlendshapeConfig(EyelidPositionBlendshape[] blendshapes, float lerpValue,
			bool relativeToCurrentValueIfUsedInOtherConfig = false)
		{
			foreach (EyelidPositionBlendshape blendShape in blendshapes)
				if (blendShape.skinnedMeshRenderer != null)
				{
					float value = Mathf.Lerp(
						(blendShape.isUsedInEalierConfig && relativeToCurrentValueIfUsedInOtherConfig)
							? blendShape.skinnedMeshRenderer.GetBlendShapeWeight(blendShape.index)
							: blendShape.defaultWeight,
						blendShape.positionWeight, lerpValue);

					blendShape.skinnedMeshRenderer.SetBlendShapeWeight(blendShape.index, value);
				}
		}


		public bool NeedsSaveDefaultBlendshapeConfig()
		{
			return blendshapesConfigs == null || blendshapesConfigs.Length == 0;
		}
		

		void ResetBlendshapeConfig(EyelidPositionBlendshape[] blendshapes)
		{
			if (blendshapes == null)
				return;

			foreach (EyelidPositionBlendshape blendShape in blendshapes)
				if (blendShape.skinnedMeshRenderer != null)
					blendShape.skinnedMeshRenderer.SetBlendShapeWeight(blendShape.index, blendShape.defaultWeight);
		}

				
		public void ResetBlendshapes(bool eyelidsFollowEyesVertically)
		{
			if (eyelidsFollowEyesVertically)
				ResetAllBlendshapesToDefault();
			else
				ResetBlendshapeConfig(blendshapesForBlinking);
		}
		
		
		void ResetAllBlendshapesToDefault()
		{
			ResetBlendshapeConfig(blendshapesForBlinking);
			ResetBlendshapeConfig(blendshapesForLookingDown);
			ResetBlendshapeConfig(blendshapesForLookingUp);
		}


		public void RestoreClosed()
		{
			if (eyeControl == EyeControl.MecanimEyeBones)
			{
				leftBoneEyeRotationLimiter.RestoreDefault();
				rightBoneEyeRotationLimiter.RestoreDefault();
			}
			else if (eyeControl == EyeControl.SelectedObjects)
			{
				leftEyeballEyeRotationLimiter.RestoreDefault();
				rightEyeballEyeRotationLimiter.RestoreDefault();
			}

			if (eyelidControl == EyelidControl.Bones)
			{
				foreach (var limiter in upperLeftLimiters)
					limiter.RestoreClosed(eyelidBoneMode);
				foreach (var limiter in upperRightLimiters)
					limiter.RestoreClosed(eyelidBoneMode);
				foreach (var limiter in lowerLeftLimiters)
					limiter.RestoreClosed(eyelidBoneMode);
				foreach (var limiter in lowerRightLimiters)
					limiter.RestoreClosed(eyelidBoneMode);
			}
			else if (eyelidControl == EyelidControl.Blendshapes)
			{
				ResetAllBlendshapesToDefault();

				if (blendshapesForBlinking != null)
				{
					foreach (EyelidPositionBlendshape blendShapeForBlinking in blendshapesForBlinking)
					{
						if (eyelidControl == EyelidControl.Blendshapes)
							blendShapeForBlinking.skinnedMeshRenderer.SetBlendShapeWeight(blendShapeForBlinking.index,
								blendShapeForBlinking.positionWeight);
					}
				}
			}
		}


		public void RestoreDefault(bool withEyelids = true)
		{
			if (eyeControl == EyeControl.MecanimEyeBones)
			{
				leftBoneEyeRotationLimiter.RestoreDefault();
				rightBoneEyeRotationLimiter.RestoreDefault();
			}
			else if (eyeControl == EyeControl.SelectedObjects)
			{
				leftEyeballEyeRotationLimiter.RestoreDefault();
				rightEyeballEyeRotationLimiter.RestoreDefault();
			}

			if (withEyelids)
				RestoreDefaultEyelids();
		}


		void RestoreDefaultEyelids()
		{
			if (eyelidControl == EyelidControl.Bones)
			{
				foreach (var limiter in upperLeftLimiters)
					limiter.RestoreDefault(eyelidBoneMode);
				foreach (var limiter in upperRightLimiters)
					limiter.RestoreDefault(eyelidBoneMode);
				foreach (var limiter in lowerLeftLimiters)
					limiter.RestoreDefault(eyelidBoneMode);
				foreach (var limiter in lowerRightLimiters)
					limiter.RestoreDefault(eyelidBoneMode);
			}
			else if (eyelidControl == EyelidControl.Blendshapes)
				ResetAllBlendshapesToDefault();
			
			wereEyelidsRestoredToDefaultSinceLastUpdate = true;
		}
		
		
		public void RestoreLookDown()
		{
			if (eyeControl == EyeControl.MecanimEyeBones)
			{
				leftBoneEyeRotationLimiter.RestoreLookDown();
				rightBoneEyeRotationLimiter.RestoreLookDown();
			}
			else if (eyeControl == EyeControl.SelectedObjects)
			{
				leftEyeballEyeRotationLimiter.RestoreLookDown();
				rightEyeballEyeRotationLimiter.RestoreLookDown();
			}

			if (eyelidControl == EyelidControl.Bones)
			{
				foreach (var limiter in upperLeftLimiters)
					limiter.RestoreLookDown(eyelidBoneMode);
				foreach (var limiter in upperRightLimiters)
					limiter.RestoreLookDown(eyelidBoneMode);
				foreach (var limiter in lowerLeftLimiters)
					limiter.RestoreLookDown(eyelidBoneMode);
				foreach (var limiter in lowerRightLimiters)
					limiter.RestoreLookDown(eyelidBoneMode);
			}
			else if (eyelidControl == EyelidControl.Blendshapes)
			{
				ResetAllBlendshapesToDefault();

				foreach (EyelidPositionBlendshape blendshapeForLookingDown in blendshapesForLookingDown)
					if (eyelidControl == EyelidControl.Blendshapes)
						blendshapeForLookingDown.skinnedMeshRenderer.SetBlendShapeWeight(blendshapeForLookingDown.index,
							blendshapeForLookingDown.positionWeight);
			}
		}


		public void RestoreLookUp()
		{
			if (eyeControl == EyeControl.MecanimEyeBones)
			{
				leftBoneEyeRotationLimiter.RestoreLookUp();
				rightBoneEyeRotationLimiter.RestoreLookUp();
			}
			else if (eyeControl == EyeControl.SelectedObjects)
			{
				leftEyeballEyeRotationLimiter.RestoreLookUp();
				rightEyeballEyeRotationLimiter.RestoreLookUp();
			}

			if (eyelidControl == EyelidControl.Bones)
			{
				foreach (var limiter in upperLeftLimiters)
					limiter.RestoreLookUp(eyelidBoneMode);
				foreach (var limiter in upperRightLimiters)
					limiter.RestoreLookUp(eyelidBoneMode);
				foreach (var limiter in lowerLeftLimiters)
					limiter.RestoreLookUp(eyelidBoneMode);
				foreach (var limiter in lowerRightLimiters)
					limiter.RestoreLookUp(eyelidBoneMode);
			}
			else if (eyelidControl == EyelidControl.Blendshapes)
			{
				ResetAllBlendshapesToDefault();

				foreach (EyelidPositionBlendshape blendshapeForLookingUp in blendshapesForLookingUp)
					if (eyelidControl == EyelidControl.Blendshapes)
						blendshapeForLookingUp.skinnedMeshRenderer.SetBlendShapeWeight(blendshapeForLookingUp.index,
							blendshapeForLookingUp.positionWeight);
			}
		}


		void SaveBlendshapesForEyelidPosition(out EyelidPositionBlendshape[] blendshapesForPosition, Object rootObject)
		{
			List<EyelidPositionBlendshape> blendshapeForPositionList = new List<EyelidPositionBlendshape>();

			if (eyelidControl == EyelidControl.Blendshapes)
			{
				SkinnedMeshRenderer[] skinnedMeshRenderers =
					(rootObject as MonoBehaviour).GetComponentsInChildren<SkinnedMeshRenderer>();
				if (skinnedMeshRenderers.Length != blendshapesConfigs.Length)
				{
					Debug.LogError(
						"The saved data for open eyelids is invalid. Please reset to open eyelids and resave 'Eyes open, looking straight'.");
					isEyelidBlendshapeDefaultSet = false;
					isEyelidBlendshapeClosedSet = false;
					isEyelidBlendshapeLookDownSet = false;
					isEyelidBlendshapeLookUpSet = false;
				}
				else
				{
					for (int i = 0; i < skinnedMeshRenderers.Length; i++)
					{
						SkinnedMeshRenderer skinnedMeshRenderer = skinnedMeshRenderers[i];
						BlendshapesConfig blendshapesConfig = blendshapesConfigs[i];
						if (skinnedMeshRenderer != blendshapesConfig.skinnedMeshRenderer ||
						    skinnedMeshRenderer.sharedMesh.blendShapeCount != blendshapesConfig.blendShapeCount)
						{
							Debug.LogError(
								"The saved data for open eyelids is invalid. Please reset to open eyelids and resave 'Eyes open, looking straight'.");
							isEyelidBlendshapeDefaultSet = false;
							isEyelidBlendshapeClosedSet = false;
							isEyelidBlendshapeLookDownSet = false;
							isEyelidBlendshapeLookUpSet = false;
						}
						else
						{
							for (int j = 0; j < blendshapesConfig.blendShapeCount; j++)
							{
								const float kEpsilon = 0.01f;
								if (Mathf.Abs(blendshapesConfig.blendshapeWeights[j] -
								              skinnedMeshRenderer.GetBlendShapeWeight(j)) >= kEpsilon)
								{
									EyelidPositionBlendshape eyePositionBlendshape = new EyelidPositionBlendshape
									{
										skinnedMeshRenderer = skinnedMeshRenderer,
										name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(j),
										index = j,
										defaultWeight = blendshapesConfig.blendshapeWeights[j],
										positionWeight = skinnedMeshRenderer.GetBlendShapeWeight(j)
									};
									blendshapeForPositionList.Add(eyePositionBlendshape);
								}
							}
						}
					}
				}
			}

			blendshapesForPosition = blendshapeForPositionList.ToArray();
		}


		public void SaveClosed(Object rootObject)
		{
			if (eyelidControl == EyelidControl.Bones)
			{
				foreach (var limiter in upperLeftLimiters)
					limiter.SaveClosed();
				foreach (var limiter in upperRightLimiters)
					limiter.SaveClosed();
				foreach (var limiter in lowerLeftLimiters)
					limiter.SaveClosed();
				foreach (var limiter in lowerRightLimiters)
					limiter.SaveClosed();

				isEyelidBonesClosedSet = true;
			}
			else if (eyelidControl == EyelidControl.Blendshapes)
			{
				isEyelidBlendshapeClosedSet = true;
				SaveBlendshapesForEyelidPosition(out blendshapesForBlinking, rootObject);
			}
			
			StoreSetup();
		}


		public void SaveDefault(Object rootObject)
		{
			if (eyeControl == EyeControl.MecanimEyeBones)
			{
				Animator animator = (rootObject as MonoBehaviour).GetComponent<Animator>();
				Transform leftEyeBoneTransform = animator.GetBoneTransform(HumanBodyBones.LeftEye);
				Transform rightEyeBoneTransform = animator.GetBoneTransform(HumanBodyBones.RightEye);

				leftBoneEyeRotationLimiter.SaveDefault(leftEyeBoneTransform);
				rightBoneEyeRotationLimiter.SaveDefault(rightEyeBoneTransform);

				isEyeBoneDefaultSet = true;
			}
			else if (eyeControl == EyeControl.SelectedObjects)
			{
				leftEyeballEyeRotationLimiter.SaveDefault(leftEye);
				rightEyeballEyeRotationLimiter.SaveDefault(rightEye);

				isEyeBallDefaultSet = true;
			}

			if (eyelidControl == EyelidControl.Bones)
			{
				for (int i = 0; i < upperLeftEyelidBones.Count; i++)
				{
					if (i >= upperLeftLimiters.Count)
						upperLeftLimiters.Add(new EyelidRotationLimiter());
					if (upperLeftLimiters[i].transform != upperLeftEyelidBones[i])
						upperLeftLimiters[i] = new EyelidRotationLimiter();
					upperLeftLimiters[i].SaveDefault(upperLeftEyelidBones[i]);
				}

				for (int i = 0; i < upperRightEyelidBones.Count; i++)
				{
					if (i >= upperRightLimiters.Count)
						upperRightLimiters.Add(new EyelidRotationLimiter());
					if (upperRightLimiters[i].transform != upperRightEyelidBones[i])
						upperRightLimiters[i] = new EyelidRotationLimiter();
					upperRightLimiters[i].SaveDefault(upperRightEyelidBones[i]);
				}

				for (int i = 0; i < lowerLeftEyelidBones.Count; i++)
				{
					if (i >= lowerLeftLimiters.Count)
						lowerLeftLimiters.Add(new EyelidRotationLimiter());
					if (lowerLeftLimiters[i].transform != lowerLeftEyelidBones[i])
						lowerLeftLimiters[i] = new EyelidRotationLimiter();
					lowerLeftLimiters[i].SaveDefault(lowerLeftEyelidBones[i]);
				}

				for (int i = 0; i < lowerRightEyelidBones.Count; i++)
				{
					if (i >= lowerRightLimiters.Count)
						lowerRightLimiters.Add(new EyelidRotationLimiter());
					if (lowerRightLimiters[i].transform != lowerRightEyelidBones[i])
						lowerRightLimiters[i] = new EyelidRotationLimiter();
					lowerRightLimiters[i].SaveDefault(lowerRightEyelidBones[i]);
				}

				isEyelidBonesDefaultSet = true;
			}
			else if (eyelidControl == EyelidControl.Blendshapes)
			{
				CreateNewBlendshapeConfigs((rootObject as MonoBehaviour).transform);

				isEyelidBlendshapeDefaultSet = true;
			}
			
			StoreSetup();
		}


		public void SaveLookDown(Object rootObject)
		{
			bool isEyeBonesControl = eyeControl == EyeControl.MecanimEyeBones;
			bool isEyeballsControl = eyeControl == EyeControl.SelectedObjects;

			if (isEyeBonesControl)
			{
				leftBoneEyeRotationLimiter.SaveLookDown();
				rightBoneEyeRotationLimiter.SaveLookDown();

				isEyeBoneLookDownSet = true;
			}
			else if (isEyeballsControl)
			{
				leftEyeballEyeRotationLimiter.SaveLookDown();
				rightEyeballEyeRotationLimiter.SaveLookDown();

				isEyeBallLookDownSet = true;
			}

			float leftMaxDownAngle = isEyeBonesControl
				? leftBoneEyeRotationLimiter.maxDownAngle
				: leftEyeballEyeRotationLimiter.maxDownAngle;
			float rightMaxDownAngle = isEyeBonesControl
				? rightBoneEyeRotationLimiter.maxDownAngle
				: rightEyeballEyeRotationLimiter.maxDownAngle;

			if (eyelidControl == EyelidControl.Bones)
			{
				foreach (var limiter in upperLeftLimiters)
					limiter.SaveLookDown(leftMaxDownAngle);
				foreach (var limiter in upperRightLimiters)
					limiter.SaveLookDown(rightMaxDownAngle);
				foreach (var limiter in lowerLeftLimiters)
					limiter.SaveLookDown(leftMaxDownAngle);
				foreach (var limiter in lowerRightLimiters)
					limiter.SaveLookDown(rightMaxDownAngle);

				isEyelidBonesLookDownSet = true;
			}
			else if (eyelidControl == EyelidControl.Blendshapes)
			{
				isEyelidBlendshapeLookDownSet = true;
				SaveBlendshapesForEyelidPosition(out blendshapesForLookingDown, rootObject);
			}
			
			StoreSetup();
		}


		public void SaveLookUp(Object rootObject)
		{
			bool isEyeBonesControl = eyeControl == EyeControl.MecanimEyeBones;
			bool isEyeballsControl = eyeControl == EyeControl.SelectedObjects;

			if (isEyeBonesControl)
			{
				leftBoneEyeRotationLimiter.SaveLookUp();
				rightBoneEyeRotationLimiter.SaveLookUp();

				isEyeBoneLookUpSet = true;
			}
			else if (isEyeballsControl)
			{
				leftEyeballEyeRotationLimiter.SaveLookUp();
				rightEyeballEyeRotationLimiter.SaveLookUp();

				isEyeBallLookUpSet = true;
			}

			float leftMaxUpAngle = isEyeBonesControl
				? leftBoneEyeRotationLimiter.maxUpAngle
				: leftEyeballEyeRotationLimiter.maxUpAngle;
			float rightMaxUpAngle = isEyeBonesControl
				? rightBoneEyeRotationLimiter.maxUpAngle
				: rightEyeballEyeRotationLimiter.maxUpAngle;

			if (eyelidControl == EyelidControl.Bones)
			{
				foreach (var limiter in upperLeftLimiters)
					limiter.SaveLookUp(leftMaxUpAngle);
				foreach (var limiter in upperRightLimiters)
					limiter.SaveLookUp(rightMaxUpAngle);
				foreach (var limiter in lowerLeftLimiters)
					limiter.SaveLookUp(leftMaxUpAngle);
				foreach (var limiter in lowerRightLimiters)
					limiter.SaveLookUp(rightMaxUpAngle);

				isEyelidBonesLookUpSet = true;
			}
			else if (eyelidControl == EyelidControl.Blendshapes)
			{
				isEyelidBlendshapeLookUpSet = true;
				SaveBlendshapesForEyelidPosition(out blendshapesForLookingUp, rootObject);
			}
			
			StoreSetup();
		}


		public void StoreSetup()
		{
			eyeControlStored = eyeControl;
			eyelidControlStored = eyelidControl;
			eyelidBoneModeStored = eyelidBoneMode;
			leftEyeStored = leftEye;
			rightEyeStored = rightEye;

			upperLeftEyelidBonesStored = new List<Transform>(upperLeftEyelidBones);
			lowerLeftEyelidBonesStored = new List<Transform>(lowerLeftEyelidBones);
			upperRightEyelidBonesStored = new List<Transform>(upperRightEyelidBones);
			lowerRightEyelidBonesStored = new List<Transform>(lowerRightEyelidBones);
			
			isEyeBoneDefaultSetStored = isEyeBoneDefaultSet;
			isEyeBallDefaultSetStored = isEyeBallDefaultSet;
			isEyelidBonesDefaultSetStored = isEyelidBonesDefaultSet;
			isEyelidBlendshapeDefaultSetStored = isEyelidBlendshapeDefaultSet;
			isEyelidBonesClosedSetStored = isEyelidBonesClosedSet;
			isEyelidBlendshapeClosedSetStored = isEyelidBlendshapeClosedSet;
			isEyeBoneLookDownSetStored = isEyeBoneLookDownSet;
			isEyeBallLookDownSetStored = isEyeBallLookDownSet;
			isEyelidBonesLookDownSetStored = isEyelidBonesLookDownSet;
			isEyelidBlendshapeLookDownSetStored = isEyelidBlendshapeLookDownSet;
			isEyeBoneLookUpSetStored = isEyeBoneLookUpSet;
			isEyeBallLookUpSetStored = isEyeBallLookUpSet;
			isEyelidBonesLookUpSetStored = isEyelidBonesLookUpSet;
			isEyelidBlendshapeLookUpSetStored = isEyelidBlendshapeLookUpSet;
			
			wasSetupStored = true;
		}


		void UpdateEyelidBonesOfOneLid(float eyeAngle, float blink01, bool isUpper, List<Transform> lidBones,
			List<EyelidRotationLimiter> limiters, float finalEyelidWeight)
		{
			for (int i = 0; i < lidBones.Count; i++)
			{
				if (i >= limiters.Count)
					break;

				Quaternion rotation;
				Vector3 position = lidBones[i].localPosition;
				limiters[i].GetRotationAndPosition(eyeAngle, blink01, eyeWidenOrSquint, isUpper, out rotation,
					ref position, eyelidBoneMode);
				if (eyelidBoneMode == EyelidBoneMode.RotationAndPosition || eyelidBoneMode == EyelidBoneMode.Rotation)
					lidBones[i].localRotation = Quaternion.Slerp(lidBones[i].localRotation, rotation, finalEyelidWeight);
				if (eyelidBoneMode == EyelidBoneMode.RotationAndPosition || eyelidBoneMode == EyelidBoneMode.Position)
					lidBones[i].localPosition = Vector3.Lerp(lidBones[i].localPosition, position, finalEyelidWeight);
			}
		}


		public void UpdateEyelids(float leftEyeAngle, float rightEyeAngle, float blink01,
			bool eyelidsFollowEyesVertically, float finalEyelidWeight)
		{
			leftEyeAngle = Utils.NormalizedDegAngle(leftEyeAngle);
			rightEyeAngle = Utils.NormalizedDegAngle(rightEyeAngle);

			if (eyelidControl == EyelidControl.Bones)
			{
				UpdateEyelidBonesOfOneLid(leftEyeAngle, blink01, true, upperLeftEyelidBones, upperLeftLimiters, finalEyelidWeight);
				UpdateEyelidBonesOfOneLid(rightEyeAngle, blink01, true, upperRightEyelidBones, upperRightLimiters, finalEyelidWeight);
				UpdateEyelidBonesOfOneLid(leftEyeAngle, blink01, false, lowerLeftEyelidBones, lowerLeftLimiters, finalEyelidWeight);
				UpdateEyelidBonesOfOneLid(rightEyeAngle, blink01, false, lowerRightEyelidBones, lowerRightLimiters, finalEyelidWeight);
			}
			else if (eyelidControl == EyelidControl.Blendshapes)
			{
				if ( false == wereEyelidsRestoredToDefaultSinceLastUpdate )
					RestoreDefaultEyelids();
				
				wereEyelidsRestoredToDefaultSinceLastUpdate = false;
				
				bool isLookingDown = leftEyeAngle > 0;
				float eyeUp01 = isLookingDown
					? 0
					: ((eyeControl == EyeControl.MecanimEyeBones)
						? leftBoneEyeRotationLimiter.GetEyeUp01(leftEyeAngle)
						: leftEyeballEyeRotationLimiter.GetEyeUp01(leftEyeAngle));
				float eyeDown01 = !isLookingDown
					? 0
					: ((eyeControl == EyeControl.MecanimEyeBones)
						? leftBoneEyeRotationLimiter.GetEyeDown01(leftEyeAngle)
						: leftEyeballEyeRotationLimiter.GetEyeDown01(leftEyeAngle));

				if (eyelidsFollowEyesVertically)
				{
					if (isLookingDown)
						LerpBlendshapeConfig(blendshapesForLookingDown, eyeDown01 * finalEyelidWeight);
					else
						LerpBlendshapeConfig(blendshapesForLookingUp, eyeUp01 * finalEyelidWeight);
				}

				if (eyeWidenOrSquint < 0)
					blink01 = Mathf.Lerp(blink01, 1, -eyeWidenOrSquint);

				LerpBlendshapeConfig(blendshapesForBlinking, blink01 * finalEyelidWeight,
					relativeToCurrentValueIfUsedInOtherConfig: eyelidsFollowEyesVertically);
			}
		}
		
		
		public void ValidateSetup()
		{
			if ( false == wasSetupStored )
				return;
			
			bool wasSetupChanged =	eyeControlStored != eyeControl ||
													eyelidControlStored != eyelidControl ||
													eyelidBoneModeStored != eyelidBoneMode ||
													leftEyeStored != leftEye ||
													rightEyeStored != rightEye ||
													upperLeftEyelidBonesStored.Count != upperLeftEyelidBones.Count ||
													lowerLeftEyelidBonesStored.Count != lowerLeftEyelidBones.Count ||
													upperRightEyelidBonesStored.Count != upperRightEyelidBones.Count ||
													lowerRightEyelidBonesStored.Count != lowerRightEyelidBones.Count;
			
			if ( false == wasSetupChanged )
				for ( int i=0;  i<upperLeftEyelidBones.Count;  i++ )
					if ( upperLeftEyelidBones[i] != upperLeftEyelidBonesStored[i] )
					{
						wasSetupChanged  = true;
						break;
					}
			if ( false == wasSetupChanged )
				for ( int i=0;  i<upperRightEyelidBones.Count;  i++ )
					if ( upperRightEyelidBones[i] != upperRightEyelidBonesStored[i] )
					{
						wasSetupChanged  = true;
						break;
					}
			if ( false == wasSetupChanged )
				for ( int i=0;  i<lowerLeftEyelidBones.Count;  i++ )
					if ( lowerLeftEyelidBones[i] != lowerLeftEyelidBonesStored[i] )
					{
						wasSetupChanged  = true;
						break;
					}
			if ( false == wasSetupChanged )
				for ( int i=0;  i<lowerRightEyelidBones.Count;  i++ )
					if ( lowerRightEyelidBones[i] != lowerRightEyelidBonesStored[i] )
					{
						wasSetupChanged  = true;
						break;
					}

			if (wasSetupChanged)
			{
				isEyeBoneDefaultSet =
					isEyeBallDefaultSet = isEyelidBonesDefaultSet = isEyelidBlendshapeDefaultSet = false;
				isEyelidBonesClosedSet = isEyelidBlendshapeClosedSet = false;
				isEyeBoneLookDownSet = isEyeBallLookDownSet =
					isEyelidBonesLookDownSet = isEyelidBlendshapeLookDownSet = false;
				isEyeBoneLookUpSet = isEyeBallLookUpSet = isEyelidBonesLookUpSet = isEyelidBlendshapeLookUpSet = false;
			}
			else
			{ 
				isEyeBoneDefaultSet = isEyeBoneDefaultSetStored;
				isEyeBallDefaultSet = isEyeBallDefaultSetStored;
				isEyelidBonesDefaultSet = isEyelidBonesDefaultSetStored;
				isEyelidBlendshapeDefaultSet = isEyelidBlendshapeDefaultSetStored;
				isEyelidBonesClosedSet = isEyelidBonesClosedSetStored;
				isEyelidBlendshapeClosedSet = isEyelidBlendshapeClosedSetStored;
				isEyeBoneLookDownSet = isEyeBoneLookDownSetStored;
				isEyeBallLookDownSet = isEyeBallLookDownSetStored;
				isEyelidBonesLookDownSet = isEyelidBonesLookDownSetStored;
				isEyelidBlendshapeLookDownSet = isEyelidBlendshapeLookDownSetStored;
				isEyeBoneLookUpSet = isEyeBoneLookUpSetStored;
				isEyeBallLookUpSet = isEyeBallLookUpSetStored;
				isEyelidBonesLookUpSet = isEyelidBonesLookUpSetStored;
				isEyelidBlendshapeLookUpSet = isEyelidBlendshapeLookUpSetStored;
			}
		}

	}
}