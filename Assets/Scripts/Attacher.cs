using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Attacher : MonoBehaviour
{
    #region Properties
    [Header("Attachments Parameters")]
    [SerializeField, ReadOnly]
    private Transform _attachmentsPoint;
    public Transform AttachmentsPoint
    {
        get => _attachmentsPoint;
        private set => _attachmentsPoint = value;
    }

    [SerializeField, ReadOnly]
    private List<GameObject> _attachmentsList;
    public List<GameObject> AttachmentsList
    {
        get => _attachmentsList;
        private set => _attachmentsList = value;
    }
    #endregion

    void Start()
    {
        AttachmentsPoint = transform.Find("Attachments");
    }

    void Update()
    {
    }

    void AddAttachment(GameObject attachment) {
        attachment.transform.parent = AttachmentsPoint;
        AttachmentsList.Add(attachment);
    }
}
