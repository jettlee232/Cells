using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class MultiplayerVRSynchronization_MT : MonoBehaviour, IPunObservable
{
    private PhotonView m_PhotonView;

    // 메인 플레이어
    [Header("Main VRPlayer Transform Synch")]
    public Transform generalVRPlayerTransform;

    private float m_Distance_GeneralVRPlayer;
    private Vector3 m_Direction_GeneralVRPlayer;
    private Vector3 m_NetworkPosition_GeneralVRPlayer;
    private Vector3 m_StoredPosition_GeneralVRPlayer;

    private Quaternion m_NetworkRotation_GeneralVRPlayer;
    private float m_Angle_GeneralVRPlayer;


    // 메인 아바타
    [Header("Main Avatar Transform Synch")]
    public Transform mainAvatarTransform;

    private float m_Distance_MainAvatar;
    private Vector3 m_Direction_MainAvatar;
    private Vector3 m_NetworkPosition_MainAvatar;
    private Vector3 m_StoredPosition_MainAvatar;

    private Quaternion m_NetworkRotation_MainAvatar;
    private float m_Angle_MainAvatar;

    // 머리
    [Header("Avatar Head Transform Synch")]
    public Transform headTransform;

    private Quaternion m_NetworkRotation_Head;
    private float m_Angle_Head;

    // 몸
    [Header("Avatar Body Transform Synch")]
    public Transform bodyTransform;

    private Quaternion m_NetworkRotation_Body;
    private float m_Angle_Body;


    // 손
    [Header("Hands Transform Synch")]
    public Transform leftHandTransform;
    public Transform rightHandTransform;

    // 왼손
    private float m_Distance_LeftHand;

    private Vector3 m_Direction_LeftHand;
    private Vector3 m_NetworkPosition_LeftHand;
    private Vector3 m_StoredPosition_LeftHand;

    private Quaternion m_NetworkRotation_LeftHand;
    private float m_Angle_LeftHand;

    // 오른손
    private float m_Distance_RightHand;

    private Vector3 m_Direction_RightHand;
    private Vector3 m_NetworkPosition_RightHand;
    private Vector3 m_StoredPosition_RightHand;

    private Quaternion m_NetworkRotation_RightHand;
    private float m_Angle_RightHand;

    bool m_firstTake = false;

    public void Awake()
    {
        m_PhotonView = GetComponent<PhotonView>();

        // VR 플레이어
        m_StoredPosition_GeneralVRPlayer = generalVRPlayerTransform.position;
        m_NetworkPosition_GeneralVRPlayer = Vector3.zero;
        m_NetworkRotation_GeneralVRPlayer = Quaternion.identity;

        // 메인 아바타
        m_StoredPosition_MainAvatar = mainAvatarTransform.localPosition;
        m_NetworkPosition_MainAvatar = Vector3.zero;
        m_NetworkRotation_MainAvatar = Quaternion.identity;

        // 머리
        m_NetworkRotation_Head = Quaternion.identity;

        // 몸
        m_NetworkRotation_Body = Quaternion.identity;

        // 왼손
        m_StoredPosition_LeftHand = leftHandTransform.localPosition;
        m_NetworkPosition_LeftHand = Vector3.zero;
        m_NetworkRotation_LeftHand = Quaternion.identity;

        // 오른손
        m_StoredPosition_RightHand = rightHandTransform.localPosition;
        m_NetworkPosition_RightHand = Vector3.zero;
        m_NetworkRotation_RightHand = Quaternion.identity;
    }

    void OnEnable()
    {
        m_firstTake = true;
    }

    public void Update()
    {
        if (!this.m_PhotonView.IsMine)
        {
            generalVRPlayerTransform.position = Vector3.MoveTowards(generalVRPlayerTransform.position, this.m_NetworkPosition_GeneralVRPlayer, this.m_Distance_GeneralVRPlayer * (1.0f / PhotonNetwork.SerializationRate));
            generalVRPlayerTransform.rotation = Quaternion.RotateTowards(generalVRPlayerTransform.rotation, this.m_NetworkRotation_GeneralVRPlayer, this.m_Angle_GeneralVRPlayer * (1.0f / PhotonNetwork.SerializationRate));

            mainAvatarTransform.localPosition = Vector3.MoveTowards(mainAvatarTransform.localPosition, this.m_NetworkPosition_MainAvatar, this.m_Distance_MainAvatar * (1.0f / PhotonNetwork.SerializationRate));
            mainAvatarTransform.localRotation = Quaternion.RotateTowards(mainAvatarTransform.localRotation, this.m_NetworkRotation_MainAvatar, this.m_Angle_MainAvatar * (1.0f / PhotonNetwork.SerializationRate));

            headTransform.localRotation = Quaternion.RotateTowards(headTransform.localRotation, this.m_NetworkRotation_Head, this.m_Angle_Head * (1.0f / PhotonNetwork.SerializationRate));

            bodyTransform.localRotation = Quaternion.RotateTowards(bodyTransform.localRotation, this.m_NetworkRotation_Body, this.m_Angle_Body * (1.0f / PhotonNetwork.SerializationRate));

            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, this.m_NetworkPosition_LeftHand, this.m_Distance_LeftHand * (1.0f / PhotonNetwork.SerializationRate));
            leftHandTransform.localRotation = Quaternion.RotateTowards(leftHandTransform.localRotation, this.m_NetworkRotation_LeftHand, this.m_Angle_LeftHand * (1.0f / PhotonNetwork.SerializationRate));

            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, this.m_NetworkPosition_RightHand, this.m_Distance_RightHand * (1.0f / PhotonNetwork.SerializationRate));
            rightHandTransform.localRotation = Quaternion.RotateTowards(rightHandTransform.localRotation, this.m_NetworkRotation_RightHand, this.m_Angle_RightHand * (1.0f / PhotonNetwork.SerializationRate));
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // VR 플레이어
            this.m_Direction_GeneralVRPlayer = generalVRPlayerTransform.position - this.m_StoredPosition_GeneralVRPlayer;
            this.m_StoredPosition_GeneralVRPlayer = generalVRPlayerTransform.position;

            stream.SendNext(generalVRPlayerTransform.position);
            stream.SendNext(this.m_Direction_GeneralVRPlayer);
            stream.SendNext(generalVRPlayerTransform.rotation);

            // 메인 아바타
            this.m_Direction_MainAvatar = mainAvatarTransform.localPosition - this.m_StoredPosition_MainAvatar;
            this.m_StoredPosition_MainAvatar = mainAvatarTransform.localPosition;

            stream.SendNext(mainAvatarTransform.localPosition);
            stream.SendNext(this.m_Direction_MainAvatar);
            stream.SendNext(mainAvatarTransform.localRotation);

            // 머리
            stream.SendNext(headTransform.localRotation);

            // 몸
            stream.SendNext(bodyTransform.localRotation);

            // 왼손
            this.m_Direction_LeftHand = leftHandTransform.localPosition - this.m_StoredPosition_LeftHand;
            this.m_StoredPosition_LeftHand = leftHandTransform.localPosition;

            stream.SendNext(leftHandTransform.localPosition);
            stream.SendNext(this.m_Direction_LeftHand);
            stream.SendNext(leftHandTransform.localRotation);

            // 오른손
            this.m_Direction_RightHand = rightHandTransform.localPosition - this.m_StoredPosition_RightHand;
            this.m_StoredPosition_RightHand = rightHandTransform.localPosition;

            stream.SendNext(rightHandTransform.localPosition);
            stream.SendNext(this.m_Direction_RightHand);
            stream.SendNext(rightHandTransform.localRotation);
        }
        else
        {
            // 다른 플레이어 받아오기

            // VR 플레이어
            this.m_NetworkPosition_GeneralVRPlayer = (Vector3)stream.ReceiveNext();
            this.m_Direction_GeneralVRPlayer = (Vector3)stream.ReceiveNext();

            if (m_firstTake)
            {
                generalVRPlayerTransform.position = this.m_NetworkPosition_GeneralVRPlayer;
                this.m_Distance_GeneralVRPlayer = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                this.m_NetworkPosition_GeneralVRPlayer += this.m_Direction_GeneralVRPlayer * lag;
                this.m_Distance_GeneralVRPlayer = Vector3.Distance(generalVRPlayerTransform.position, this.m_NetworkPosition_GeneralVRPlayer);
            }

            // 메인
            this.m_NetworkRotation_GeneralVRPlayer = (Quaternion)stream.ReceiveNext();
            if (m_firstTake)
            {
                this.m_Angle_GeneralVRPlayer = 0f;
                generalVRPlayerTransform.rotation = this.m_NetworkRotation_GeneralVRPlayer;
            }
            else
            {
                this.m_Angle_GeneralVRPlayer = Quaternion.Angle(generalVRPlayerTransform.rotation, this.m_NetworkRotation_GeneralVRPlayer);
            }

            this.m_NetworkPosition_MainAvatar = (Vector3)stream.ReceiveNext();
            this.m_Direction_MainAvatar = (Vector3)stream.ReceiveNext();

            if (m_firstTake)
            {
                mainAvatarTransform.localPosition = this.m_NetworkPosition_MainAvatar;
                this.m_Distance_MainAvatar = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                this.m_NetworkPosition_MainAvatar += this.m_Direction_MainAvatar * lag;
                this.m_Distance_MainAvatar = Vector3.Distance(mainAvatarTransform.localPosition, this.m_NetworkPosition_MainAvatar);
            }

            this.m_NetworkRotation_MainAvatar = (Quaternion)stream.ReceiveNext();
            if (m_firstTake)
            {
                this.m_Angle_MainAvatar = 0f;
                mainAvatarTransform.rotation = this.m_NetworkRotation_MainAvatar;
            }
            else
            {
                this.m_Angle_MainAvatar = Quaternion.Angle(mainAvatarTransform.rotation, this.m_NetworkRotation_MainAvatar);
            }

            // 머리
            this.m_NetworkRotation_Head = (Quaternion)stream.ReceiveNext();
            if (m_firstTake)
            {
                this.m_Angle_Head = 0f;
                headTransform.localRotation = this.m_NetworkRotation_Head;
            }
            else
            {
                this.m_Angle_Head = Quaternion.Angle(headTransform.localRotation, this.m_NetworkRotation_Head);
            }

            // 몸
            this.m_NetworkRotation_Body = (Quaternion)stream.ReceiveNext();
            if (m_firstTake)
            {
                this.m_Angle_Body = 0f;
                bodyTransform.localRotation = this.m_NetworkRotation_Body;
            }
            else
            {
                this.m_Angle_Body = Quaternion.Angle(bodyTransform.localRotation, this.m_NetworkRotation_Body);
            }

            // 왼손
            this.m_NetworkPosition_LeftHand = (Vector3)stream.ReceiveNext();
            this.m_Direction_LeftHand = (Vector3)stream.ReceiveNext();
            if (m_firstTake)
            {
                leftHandTransform.localPosition = this.m_NetworkPosition_LeftHand;
                this.m_Distance_LeftHand = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                this.m_NetworkPosition_LeftHand += this.m_Direction_LeftHand * lag;
                this.m_Distance_LeftHand = Vector3.Distance(leftHandTransform.localPosition, this.m_NetworkPosition_LeftHand);
            }
            this.m_NetworkRotation_LeftHand = (Quaternion)stream.ReceiveNext();
            if (m_firstTake)
            {
                this.m_Angle_LeftHand = 0f;
                leftHandTransform.localRotation = this.m_NetworkRotation_LeftHand;
            }
            else
            {
                this.m_Angle_LeftHand = Quaternion.Angle(leftHandTransform.localRotation, this.m_NetworkRotation_LeftHand);
            }

            // 오른손
            this.m_NetworkPosition_RightHand = (Vector3)stream.ReceiveNext();
            this.m_Direction_RightHand = (Vector3)stream.ReceiveNext();

            if (m_firstTake)
            {
                rightHandTransform.localPosition = this.m_NetworkPosition_RightHand;
                this.m_Distance_RightHand = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                this.m_NetworkPosition_RightHand += this.m_Direction_RightHand * lag;
                this.m_Distance_RightHand = Vector3.Distance(rightHandTransform.localPosition, this.m_NetworkPosition_RightHand);
            }
            this.m_NetworkRotation_RightHand = (Quaternion)stream.ReceiveNext();
            if (m_firstTake)
            {
                this.m_Angle_RightHand = 0f;
                rightHandTransform.localRotation = this.m_NetworkRotation_RightHand;
            }
            else
            {
                this.m_Angle_RightHand = Quaternion.Angle(rightHandTransform.localRotation, this.m_NetworkRotation_RightHand);
            }
            if (m_firstTake)
            {
                m_firstTake = false;
            }
        }
    }
}
