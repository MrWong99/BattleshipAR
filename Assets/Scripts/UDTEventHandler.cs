/*============================================================================== 
 * Copyright (c) 2015 Qualcomm Connected Experiences, Inc. All Rights Reserved. 
 * ==============================================================================*/
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Vuforia;

public class UDTEventHandler : MonoBehaviour, IUserDefinedTargetEventHandler
{
    #region PUBLIC_MEMBERS
    /// <summary>
    /// Can be set in the Unity inspector to reference a ImageTargetBehaviour that is instanciated for augmentations of new user defined targets.
    /// </summary>
    public ImageTargetBehaviour ImageTargetTemplate;
    public GameObject board;

    public int LastTargetIndex
    {
        get { return (mTargetCounter - 1) % MAX_TARGETS; }
    }
    #endregion PUBLIC_MEMBERS


    #region PRIVATE_MEMBERS
    private const int MAX_TARGETS = 5;
    private UserDefinedTargetBuildingBehaviour mTargetBuildingBehaviour;
    private ObjectTracker mObjectTracker;

    // DataSet that newly defined targets are added to
    private DataSet mBuiltDataSet;

    // Currently observed frame quality
    private ImageTargetBuilder.FrameQuality mFrameQuality = ImageTargetBuilder.FrameQuality.FRAME_QUALITY_NONE;

    // Counter used to name newly created targets
    private int mTargetCounter;
    #endregion //PRIVATE_MEMBERS


    #region MONOBEHAVIOUR_METHODS
    public void Start()
    {
        mTargetBuildingBehaviour = GetComponent<UserDefinedTargetBuildingBehaviour>();
        if (mTargetBuildingBehaviour)
        {
            mTargetBuildingBehaviour.RegisterEventHandler(this);
            Debug.Log("Registering User Defined Target event handler.");
        }
    }
    #endregion //MONOBEHAVIOUR_METHODS


    #region IUserDefinedTargetEventHandler implementation
    /// <summary>
    /// Called when UserDefinedTargetBuildingBehaviour has been initialized successfully
    /// </summary>
    public void OnInitialized()
    {
        mObjectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        if (mObjectTracker != null)
        {
            // Create a new dataset
            mBuiltDataSet = mObjectTracker.CreateDataSet();
            mObjectTracker.ActivateDataSet(mBuiltDataSet);
        }
    }

    /// <summary>
    /// Updates the current frame quality
    /// </summary>
    public void OnFrameQualityChanged(ImageTargetBuilder.FrameQuality frameQuality)
    {
        mFrameQuality = frameQuality;
        if (mFrameQuality == ImageTargetBuilder.FrameQuality.FRAME_QUALITY_LOW)
        {
            Debug.Log("Low camera image quality");
        }
    }

    /// <summary>
    /// Takes a new trackable source and adds it to the dataset
    /// This gets called automatically as soon as you 'BuildNewTarget with UserDefinedTargetBuildingBehaviour
    /// </summary>
    public void OnNewTrackableSource(TrackableSource trackableSource)
    {
        mTargetCounter++;

        // Deactivates the dataset first
        mObjectTracker.DeactivateDataSet(mBuiltDataSet);

        // Destroy the oldest target if the dataset is full or the dataset 
        // already contains five user-defined targets.
        if (mBuiltDataSet.HasReachedTrackableLimit() || mBuiltDataSet.GetTrackables().Count() >= MAX_TARGETS)
        {
            IEnumerable<Trackable> trackables = mBuiltDataSet.GetTrackables();
            Trackable oldest = null;
            foreach (Trackable trackable in trackables)
            {
                if (oldest == null || trackable.ID < oldest.ID)
                    oldest = trackable;
            }

            if (oldest != null)
            {
                Debug.Log("Destroying oldest trackable in UDT dataset: " + oldest.Name);
                mBuiltDataSet.Destroy(oldest, true);
            }
        }

        // Get predefined trackable and instantiate it
        ImageTargetBehaviour imageTargetCopy = (ImageTargetBehaviour)Instantiate(ImageTargetTemplate);
        imageTargetCopy.gameObject.name = "UserDefinedTarget-" + mTargetCounter;

        // Add the duplicated trackable to the data set and activate it
        mBuiltDataSet.CreateTrackable(trackableSource, imageTargetCopy.gameObject);

        // Activate the dataset again
        mObjectTracker.ActivateDataSet(mBuiltDataSet);
        
        mObjectTracker.Stop();
        mObjectTracker.ResetExtendedTracking();
        mObjectTracker.Start();
    }
    #endregion IUserDefinedTargetEventHandler implementation


    #region PUBLIC_METHODS
    /// <summary>
    /// Instantiates a new user-defined target and is also responsible for dispatching callback to 
    /// IUserDefinedTargetEventHandler::OnNewTrackableSource
    /// </summary>
    public void BuildNewTarget()
    {
        if (mFrameQuality == ImageTargetBuilder.FrameQuality.FRAME_QUALITY_MEDIUM ||
            mFrameQuality == ImageTargetBuilder.FrameQuality.FRAME_QUALITY_HIGH)
        {
            // create the name of the next target.
            // the TrackableName of the original, linked ImageTargetBehaviour is extended with a continuous number to ensure unique names
            string targetName = string.Format("{0}-{1}", ImageTargetTemplate.TrackableName, mTargetCounter);
            board.SetActive(true);
            // generate a new target:
            mTargetBuildingBehaviour.BuildNewTarget(targetName, ImageTargetTemplate.GetSize().x);
        }
        else
        {
            Debug.Log("Cannot build new target, due to poor camera image quality");
        }
    }
    #endregion //PUBLIC_METHODS
}