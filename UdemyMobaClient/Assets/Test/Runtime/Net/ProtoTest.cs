using Game.Net;
using ProtoMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoTest : MonoBehaviour
{
    #region private-field
    [SerializeField]
    private string _account;
    [SerializeField]
    private string _password;
    #endregion private-field

    #region public-method
    public void SendLogin()
    {
        var userInfo = new UserInfo();
        userInfo.Account = _account;
        userInfo.Password = _password;

        var userRegisterC2S = new UserRegisterC2S();
        userRegisterC2S.UserInfo = userInfo;

        var bufferEntity = BufferFactory.CreateAndSendPackage(1001, userRegisterC2S);

        var userRegisterC2S1 = ProtobufHelper.FromBytes<UserRegisterC2S>(bufferEntity.Protocal);
    }
	#endregion public-method
}
