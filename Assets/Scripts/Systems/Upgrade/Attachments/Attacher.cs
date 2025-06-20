using System.Collections.Generic;
using System.Linq;
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
    private List<Attacheable> _attachmentsList;
    public List<Attacheable> AttachmentsList
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

    public void AddAttachment(Attacheable attachment) {
        attachment.gameObject.transform.parent = AttachmentsPoint;
        attachment.gameObject.transform.position = AttachmentsPoint.position;
        AttachmentsList.Add(attachment);
    }

    public List<Attacheable> GetAttachmentsByType<T>()
    {
        return AttachmentsList.FindAll(a => a.GetType() == typeof(T)).ToList();
    }
}
