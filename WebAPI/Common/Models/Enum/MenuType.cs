namespace WebAPI.Common.Models.Enum
{
    using System;

    [Flags]
    public enum MenuType
    {
        /// <summary>
        /// 目录
        /// </summary>
        Folder = 0,

        /// <summary>
        /// 菜单
        /// </summary>
        Menu = 1,

        /// <summary>
        /// 按钮
        /// </summary>
        Button = 2,
    }
}
