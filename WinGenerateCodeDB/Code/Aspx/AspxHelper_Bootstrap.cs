﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinGenerateCodeDB.Cache;

namespace WinGenerateCodeDB.Code
{
    public class AspxHelper_Bootstrap
    {
        public static string CreateASPX()
        {
            StringBuilder aspxContent = new StringBuilder();
            aspxContent.Append(CreatePageHead());
            aspxContent.Append(CreateHeader());
            aspxContent.Append(CreateBodyHead());
            aspxContent.Append(CreateSearchContent());
            aspxContent.Append(CreateCmdToolBar());
            aspxContent.Append(CreateDataGrid());
            aspxContent.Append(CreateDialog());
            aspxContent.Append(CreateNotifyMsg());
            aspxContent.Append(CreateJsDateFormat());
            aspxContent.Append(CreateJsOperation());
            aspxContent.Append(CreateJsLoad());
            aspxContent.Append(CreateBottomContent());

            return aspxContent.ToString();
        }

        private static string CreatePageHead()
        {
            return string.Format(@"
<%@ Page Language=""C#"" AutoEventWireup=""true"" CodeBehind=""{0}.aspx.cs"" Inherits=""{1}.{0}"" ValidateRequest=""false"" %>

<!DOCTYPE html>
<html xmlns=""http://www.w3.org/1999/xhtml"">
", PageCache.TableName_UI, PageCache.NameSpaceStr);
        }

        private static string CreateHeader()
        {
            return string.Format(@"
<head>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
    <title>{0}</title>
    <link href=""http://cdn.bootcss.com/bootstrap/3.3.7/css/bootstrap.min.css"" rel=""stylesheet"" />
    <link href=""http://cdn.bootcss.com/bootstrap/3.3.7/css/bootstrap-theme.min.css"" rel=""stylesheet"" />
    <link href=""http://cdn.bootcss.com/bootstrap-datetimepicker/2.1.30/css/bootstrap-datetimepicker.min.css"" rel=""stylesheet"" />
    <link href=""http://cdn.bootcss.com/jquery-confirm/2.0.0/jquery-confirm.min.css"" rel=""stylesheet""/>
    <script src=""http://cdn.bootcss.com/jquery/3.1.1/jquery.min.js""></script>
    <script src=""http://cdn.bootcss.com/jquery-confirm/2.0.0/jquery-confirm.min.js""></script>
    <script src=""http://cdn.bootcss.com/bootstrap/3.3.7/js/bootstrap.min.js""></script>
    <script src=""http://cdn.bootcss.com/bootstrap-datetimepicker/2.1.30/js/bootstrap-datetimepicker.min.js""></script>
    <script src=""http://cdn.bootcss.com/bootstrap-datetimepicker/2.1.30/js/locales/bootstrap-datetimepicker.zh-CN.js""></script>
</head>
", PageCache.TiTle);
        }

        private static string CreateBodyHead()
        {
            return string.Format(@"
<body>
    <div class=""container"">
        <h2>{0}
        </h2>
", PageCache.TiTle);
        }

        private static string CreateSearchContent()
        {
            StringBuilder searchContent = new StringBuilder();
            int index = 0;
            var showModel = PageCache.GetCmd("主显示");
            if (showModel != null)
            {
                foreach (var item in showModel.AttrList)
                {
                    if (index % 3 == 0)
                    {
                        if (index == 0)
                        {
                            searchContent.Append(@"
        <div class=""row form-group"">");
                        }
                        else
                        {
                            searchContent.Append(@"
        </div>
        <div class=""row form-group"">");
                        }
                    }

                    if (item.DbType.ToLower() == "datetime")
                    {
                        string attribute = item.AttrName;
                        searchContent.AppendFormat(@"
            <div class=""col-md-4"">
                {1}：<input size=""10"" type=""text"" value="""" id=""txtSearch{0}"" placeholder=""选择指定日期"" />
                <script type=""text/javascript"">
                    $('#txtSearch{0}').datetimepicker({{
                        // minView: ""month"", //选择日期后，不会再跳转去选择时分秒 
                        format: ""yyyy-mm-dd hh:ii:ss"", //选择日期后，文本框显示的日期格式 
                        language: 'zh-CN', //汉化 
                        autoclose: true //选择日期后自动关闭 
                    }});
                </script>
            </div>", attribute, item.TitleName);
                    }
                    else if (item.DbType.ToLower() == "date")
                    {
                        string attribute = item.AttrName;
                        searchContent.AppendFormat(@"
            <div class=""col-md-4"">
                {1}：<input size=""10"" type=""text"" value="""" id=""txtSearch{0}"" placeholder=""选择指定日期"" />
                <script type=""text/javascript"">
                    $('#txtSearch{0}').datetimepicker({{
                        minView: ""month"", //选择日期后，不会再跳转去选择时分秒 
                        format: ""yyyy-mm-dd"", //选择日期后，文本框显示的日期格式 
                        language: 'zh-CN', //汉化 
                        autoclose: true //选择日期后自动关闭 
                    }}).on('changeDate', function (ev) {{
                        loadMsg(1, pageSize);
                    }});
                </script>
            </div>", attribute, item.TitleName);
                    }
                    else
                    {
                        string attribute = item.AttrName;
                        searchContent.AppendFormat(@"
            <div class=""col-md-4"">
                <label for=""txtSearch{0}"">{1}：</label><input type=""text"" id=""txtSearch{0}"" />
            </div>", attribute, item.TitleName);
                    }

                    index++;
                }

                if (index > 0)
                {
                    if (index % 3 == 1)
                    {
                        searchContent.Append(@"
            <div class=""col-md-8""></div>
        </div>");
                    }
                    else if (index % 3 == 2)
                    {
                        searchContent.Append(@"
            <div class=""col-md-4""></div>
        </div>");
                    }
                    else
                    {
                        searchContent.Append(@"
        </div>");
                    }
                }

                if (searchContent.ToString() != string.Empty)
                {
                    // 非空。添加查询按钮
                    searchContent.Append(@"
        <div class=""row form-group"">
            <div class=""col-md-4""><input type=""button"" id=""btnsearch"" value=""查询"" /></div>
            <div class=""col-md-8""></div>
        </div>");
                }

                return searchContent.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        private static string CreateCmdToolBar()
        {
            int itemCount = 0;
            StringBuilder toolBarContent = new StringBuilder();
            if (PageCache.GetCmd("添加") != null)
            {
                toolBarContent.Append(@"
            <input type=""button"" value=""新增"" onclick='newModel();' />");
                itemCount++;
            }

            if (PageCache.GetCmd("编辑") != null)
            {
                toolBarContent.Append(@"
            <input type=""button"" value=""编辑"" onclick='editModel();' />");
                itemCount++;
            }

            if (PageCache.GetCmd("批量编辑") != null)
            {
                toolBarContent.Append(@"
            <input type=""button"" value=""批量编辑"" onclick='batEditModel();' />");
                itemCount++;
            }

            if (PageCache.GetCmd("删除") != null)
            {
                toolBarContent.Append(@"
            <input type=""button"" value=""删除"" onclick='destroyModel();' />");
                itemCount++;
            }

            if (PageCache.GetCmd("删除选中") != null)
            {
                toolBarContent.Append(@"
            <input type=""button"" value=""删除选中"" onclick='destroyBatModel();' />");
                itemCount++;
            }

            if (PageCache.GetCmd("导出选中") != null)
            {
                toolBarContent.Append(@"
            <input type=""button"" value=""选中导出"" onclick='selectExport();' />");
                itemCount++;
            }

            if (PageCache.GetCmd("导出全部") != null)
            {
                toolBarContent.Append(@"
            <input type=""button"" value=""导出全部"" onclick='exportAll();' />");
                itemCount++;
            }

            if (itemCount > 0)
            {
                string extendStr = string.Empty;
                int count = (int)Math.Ceiling(itemCount * 1.5);
                if (count - 12 > 0)
                {
                    extendStr = "<div class=\"col-md-" + (count - 12) + "\"></div>";
                }

                return @"
        <div class=""row form-group"">
            <div class=""col-md-" + count + @""">" + toolBarContent.ToString() + @"
            </div>" + extendStr + @"
        </div>";
            }
            else
            {
                return string.Empty;
            }
        }

        private static string CreateDataGrid()
        {
            // singleselect=""true""
            string template = @"
        <table id=""tbcontent"" class=""table table-striped"">
            <thead>
                <tr>{0}
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
        <div id=""msgPager""></div>
";

            StringBuilder tbodyContent = new StringBuilder();
            tbodyContent.Append(@"
                    <th><input type=""checkbox"" id=""chkAll"" onclick=""checkAll();"" /></th>");
            var showModel = PageCache.GetCmd("主显示");
            if (showModel != null)
            {
                foreach (var item in showModel.AttrList)
                {
                    string attribute = item.AttrName;

                    tbodyContent.AppendFormat(@"
                    <th>{0}</th>", item.TitleName);
                }

                return string.Format(template, tbodyContent.ToString());
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static string CreateDialog()
        {
            StringBuilder dialogContent = new StringBuilder();
            int addWidth = 400;
            #region 添加
            var addModel = PageCache.GetCmd("添加");
            if (addModel != null)
            {
                // 行数过多，分成两行
                string template = @"

        <!-- 模态框（Modal） -->
        <div class=""modal fade"" id=""add_Modal"" tabindex=""-1"" role=""dialog""
            aria-labelledby=""myModalLabel"" aria-hidden=""true"">
            <div class=""modal-dialog"" style=""width: auto; max-width: {1}px;"">
                <div class=""modal-content"">
                    <div class=""modal-header"">
                        <button type=""button"" class=""close""
                            data-dismiss=""modal"" aria-hidden=""true""></button>
                        <h4>添加</h4>
                    </div>
                    <div class=""modal-body"">
                        <div class=""container"">{0}
                        </div>
                    </div>
                    <div class=""modal-footer"">
                        <button type=""button"" class=""btn btn-default"" 
                            onclick=""saveAddModel()"">
                            添加
                        </button>
                        <button type=""button"" class=""btn btn-default""
                            data-dismiss=""modal"">
                            关闭
                        </button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal -->
        </div>";

                // <=8 单列， <=18&&>8 双列，>18 三列
                int index = 0;
                StringBuilder content = new StringBuilder();
                if (addModel.AttrList.Count > 18)
                {
                    #region 三列
                    content.Append(@"
                        <div class=""row form-group"">");
                    content.AppendFormat(@"
                            <input type=""hidden"" id=""txtAdd{0}"" value="""" />", PageCache.KeyId);
                    foreach (var item in addModel.AttrList)
                    {
                        if (index % 3 == 0 && index != 0)
                        {
                            content.Append(@"
                        </div>
                        <div class=""row form-group"">");
                        }

                        string attribute = item.AttrName;

                        // 初始化form显示数据
                        // &#12288; 占一个中文字符
                        content.AppendFormat(@"
                            <div class=""col-md-4"">
                                <label for=""txtAdd{0}"">{1}:</label>
                                <input type=""text"" id=""txtAdd{0}"" name=""txtAdd{0}"" />
                            </div>", attribute, item.TitleName.PadLeftStr(4, "&emsp;"));

                        index++;
                    }

                    if (index % 3 == 0)
                    {
                        content.Append(@"
                        </div>");
                    }
                    else if (index % 3 == 1)
                    {
                        content.Append(@"
                            <div class=""col-md-8""></div>
                        </div>");
                    }
                    else
                    {
                        content.Append(@"
                            <div class=""col-md-4""></div>
                        </div>");
                    }
                    #endregion

                    addWidth *= 3;
                }
                else if (addModel.AttrList.Count <= 18 & addModel.AttrList.Count > 8)
                {
                    #region 二列
                    content.Append(@"
                        <div class=""row form-group"">");
                    content.AppendFormat(@"
                            <input type=""hidden"" id=""txtAdd{0}"" value="""" />", PageCache.KeyId);
                    foreach (var item in addModel.AttrList)
                    {
                        if (index % 2 == 0 && index != 0)
                        {
                            content.Append(@"
                        </div>
                        <div class=""row form-group"">");
                        }

                        string attribute = item.AttrName;

                        // 初始化form显示数据
                        // &#12288; 占一个中文字符
                        content.AppendFormat(@"
                            <div class=""col-md-4"">
                                <label for=""txtAdd{0}"">{1}:</label>
                                <input type=""text"" id=""txtAdd{0}"" name=""txtAdd{0}"" />
                            </div>", attribute, item.TitleName.PadLeftStr(4, "&emsp;"));

                        index++;
                    }

                    if (index % 2 == 0)
                    {
                        content.Append(@"
                        </div>");
                    }
                    else
                    {
                        content.Append(@"
                            <div class=""col-md-4""></div>
                        </div>");
                    }
                    #endregion

                    addWidth *= 2;
                }
                else
                {
                    #region 一列
                    content.AppendFormat(@"
                            <input type=""hidden"" id=""txtAdd{0}"" value="""" />", PageCache.KeyId);
                    foreach (var item in addModel.AttrList)
                    {
                        content.Append(@"
                        <div class=""row form-group"">");
                        string attribute = item.AttrName;

                        // 初始化form显示数据
                        // &#12288; 占一个中文字符
                        content.AppendFormat(@"
                            <div class=""col-md-12"">
                                <label for=""txtAdd{0}"">{1}:</label>
                                <input type=""text"" id=""txtAdd{0}"" name=""txtAdd{0}"" />
                            </div>", attribute, item.TitleName.PadLeftStr(4, "&emsp;"));

                        content.Append(@"
                        </div>");
                    }
                    #endregion

                    addWidth *= 1;
                }

                dialogContent.Append(string.Format(template, content.ToString(), addWidth));
            }
            #endregion

            int editWidth = 400;
            #region 编辑
            var editModel = PageCache.GetCmd("编辑");
            if (editModel != null)
            {
                // 行数过多，分成两行
                string template = @"

        <!-- 模态框（Modal） -->
        <div class=""modal fade"" id=""edit_Modal"" tabindex=""-1"" role=""dialog""
            aria-labelledby=""myModalLabel"" aria-hidden=""true"">
            <div class=""modal-dialog"" style=""width: auto; max-width: {1}px;"">
                <div class=""modal-content"">
                    <div class=""modal-header"">
                        <button type=""button"" class=""close""
                            data-dismiss=""modal"" aria-hidden=""true""></button>
                        <h4>编辑</h4>
                    </div>
                    <div class=""modal-body"">
                        <div class=""container"">{0}
                        </div>
                    </div>
                    <div class=""modal-footer"">
                        <button type=""button"" class=""btn btn-default"" 
                            onclick=""saveEditModel()"">
                            保存
                        </button>
                        <button type=""button"" class=""btn btn-default""
                            data-dismiss=""modal"">
                            关闭
                        </button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal -->
        </div>";

                int index = 0;
                StringBuilder content = new StringBuilder();
                if (editModel.AttrList.Count > 18)
                {
                    #region 三列
                    content.Append(@"
                        <div class=""row form-group"">");

                    content.AppendFormat(@"
                            <input type=""hidden"" id=""txtEdit{0}"" value="""" />", PageCache.KeyId);
                    foreach (var item in editModel.AttrList)
                    {
                        if (index % 3 == 0 && index != 0)
                        {
                            content.Append(@"
                        </div>
                        <div class=""row form-group"">");
                        }

                        string attribute = item.AttrName;

                        // 初始化form显示数据
                        // &#12288; 占一个中文字符
                        content.AppendFormat(@"
                            <div class=""col-md-4"">
                                <label for=""txtEdit{0}"">{1}:</label>
                                <input type=""text"" id=""txtEdit{0}"" name=""txtEdit{0}"" />
                            </div>", attribute, item.TitleName.PadLeftStr(4, "&emsp;"));

                        index++;
                    }

                    if (index % 3 == 0)
                    {
                        content.Append(@"
                        </div>");
                    }
                    else if (index % 3 == 1)
                    {
                        content.Append(@"
                            <div class=""col-md-8""></div>
                        </div>");
                    }
                    else
                    {
                        content.Append(@"
                            <div class=""col-md-4""></div>
                        </div>");
                    }
                    #endregion

                    editWidth *= 3;
                }
                else if (editModel.AttrList.Count <= 18 & editModel.AttrList.Count > 8)
                {
                    #region 二列
                    content.Append(@"
                        <div class=""row form-group"">");

                    content.AppendFormat(@"
                            <input type=""hidden"" id=""txtEdit{0}"" value="""" />", PageCache.KeyId);
                    foreach (var item in editModel.AttrList)
                    {
                        if (index % 2 == 0 && index != 0)
                        {
                            content.Append(@"
                        </div>
                        <div class=""row form-group"">");
                        }

                        string attribute = item.AttrName;

                        // 初始化form显示数据
                        // &#12288; 占一个中文字符
                        content.AppendFormat(@"
                            <div class=""col-md-4"">
                                <label for=""txtEdit{0}"">{1}:</label>
                                <input type=""text"" id=""txtEdit{0}"" name=""txtEdit{0}"" />
                            </div>", attribute, item.TitleName.PadLeftStr(4, "&emsp;"));

                        index++;
                    }

                    if (index % 3 == 0)
                    {
                        content.Append(@"
                        </div>");
                    }
                    else
                    {
                        content.Append(@"
                            <div class=""col-md-4""></div>
                        </div>");
                    }
                    #endregion

                    editWidth *= 2;
                }
                else
                {
                    #region 一列
                    content.Append(@"
                        <div class=""row form-group"">");

                    content.AppendFormat(@"
                            <input type=""hidden"" id=""txtEdit{0}"" value="""" />
                        </div>", PageCache.KeyId);
                    foreach (var item in editModel.AttrList)
                    {
                        content.Append(@"
                        <div class=""row form-group"">");

                        string attribute = item.AttrName;

                        // 初始化form显示数据
                        // &#12288; 占一个中文字符
                        content.AppendFormat(@"
                            <div class=""col-md-12"">
                                <label for=""txtEdit{0}"">{1}:</label>
                                <input type=""text"" id=""txtEdit{0}"" name=""txtEdit{0}"" />
                            </div>", attribute, item.TitleName.PadLeftStr(4, "&emsp;"));

                        index++;
                        content.Append(@"
                        </div>");
                    }
                    #endregion

                    editWidth *= 1;
                }

                dialogContent.Append(string.Format(template, content.ToString(), editWidth));
            }
            #endregion

            int batEditWidth = 400;
            #region 批量编辑
            var batEditModel = PageCache.GetCmd("批量编辑");
            if (batEditModel != null)
            {
                // 行数过多，分成两行
                string template = @"

        <!-- 模态框（Modal） -->
        <div class=""modal fade"" id=""batEdit_Modal"" tabindex=""-1"" role=""dialog""
            aria-labelledby=""myModalLabel"" aria-hidden=""true"">
            <div class=""modal-dialog"" style=""width: auto; max-width: {1}px;"">
                <div class=""modal-content"">
                    <div class=""modal-header"">
                        <button type=""button"" class=""close""
                            data-dismiss=""modal"" aria-hidden=""true""></button>
                        <h4>批量编辑</h4>
                    </div>
                    <div class=""modal-body"">
                        <div class=""container"">{0}
                        </div>
                    </div>
                    <div class=""modal-footer"">
                        <button type=""button"" class=""btn btn-default"" 
                            onclick=""saveBatEditModel()"">
                            保存
                        </button>
                        <button type=""button"" class=""btn btn-default""
                            data-dismiss=""modal"">
                            关闭
                        </button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal -->
        </div>";
                int index = 1;
                StringBuilder content = new StringBuilder();
                if (batEditModel.AttrList.Count > 18)
                {
                    #region 三列
                    content.Append(@"
                        <div class=""row form-group"">");
                    content.AppendFormat(@"
                            <input type=""hidden"" id=""txtBatEdit{0}"" value="""" />", PageCache.KeyId);
                    foreach (var item in batEditModel.AttrList)
                    {
                        if (index % 3 == 0 && index != 0)
                        {
                            content.Append(@"
                        </div>
                        <div class=""row form-group"">");
                        }

                        string attribute = item.AttrName;

                        // &#12288; 占一个中文字符
                        content.AppendFormat(@"
                            <div class=""col-md-4"">
                                <label for=""txtBatEdit{0}"">{1}:</label>
                                <input type=""text"" id=""txtBatEdit{0}"" name=""txtBatEdit{0}"" />
                            </div>", attribute, item.TitleName.PadLeftStr(4, "&emsp;"));

                        index++;
                    }

                    if (index % 3 == 0)
                    {
                        content.Append(@"
                        </div>");
                    }
                    else if (index % 3 == 1)
                    {
                        content.Append(@"
                        <div class=""col-md-8""></div>
                        </div>");
                    }
                    else
                    {
                        content.Append(@"
                        <div class=""col-md-4""></div>
                        </div>");
                    }
                    #endregion

                    batEditWidth *= 3;
                }
                else if (batEditModel.AttrList.Count <= 18 & batEditModel.AttrList.Count > 8)
                {
                    #region 二列
                    content.Append(@"
                        <div class=""row form-group"">");
                    content.AppendFormat(@"
                            <input type=""hidden"" id=""txtBatEdit{0}"" value="""" />", PageCache.KeyId);
                    foreach (var item in batEditModel.AttrList)
                    {
                        if (index % 2 == 0 && index != 0)
                        {
                            content.Append(@"
                        </div>
                        <div class=""row form-group"">");
                        }

                        string attribute = item.AttrName;

                        // &#12288; 占一个中文字符
                        content.AppendFormat(@"
                            <div class=""col-md-4"">
                                <label for=""txtBatEdit{0}"">{1}:</label>
                                <input type=""text"" id=""txtBatEdit{0}"" name=""txtBatEdit{0}"" />
                            </div>", attribute, item.TitleName.PadLeftStr(4, "&emsp;"));

                        index++;
                    }

                    if (index % 2 == 0)
                    {
                        content.Append(@"
                        </div>");
                    }
                    else
                    {
                        content.Append(@"
                        <div class=""col-md-4""></div>
                        </div>");
                    }
                    #endregion

                    batEditWidth *= 2;
                }
                else
                {
                    #region 一列
                    content.Append(@"
                        <div class=""row form-group"">");
                    content.AppendFormat(@"
                            <input type=""hidden"" id=""txtBatEdit{0}"" value="""" /></div>", PageCache.KeyId);
                    foreach (var item in batEditModel.AttrList)
                    {
                        content.Append(@"
                        <div class=""row form-group"">");

                        string attribute = item.AttrName;

                        // &#12288; 占一个中文字符
                        content.AppendFormat(@"
                            <div class=""col-md-12"">
                                <label for=""txtBatEdit{0}"">{1}:</label>
                                <input type=""text"" id=""txtBatEdit{0}"" name=""txtBatEdit{0}""/>
                            </div>", attribute, item.TitleName.PadLeftStr(4, "&emsp;")); // &ensp;  &emsp; &#12288;

                        content.Append(@"
                        </div>");
                    }
                    #endregion

                    batEditWidth *= 1;
                }

                dialogContent.Append(string.Format(template, content.ToString(), batEditWidth));
            }
            #endregion

            return dialogContent.ToString();
            //}
        }

        private static string CreateNotifyMsg()
        {
            return @"

        <div id=""dialog"" class=""modal-dialog"" title=""提示"">
            <p style=""text-align: center;"">
            </p>
        </div>
    ";
        }

        /// <summary>	
        /// </summary>
        /// <returns></returns>
        private static string CreateJsDateFormat()
        {
            return @"
        <script type=""text/javascript"">
            function checkAll() {{
                var result = $(""#chkAll"")[0].checked;
                $(""#tbcontent tbody"").find(""input[type='checkbox']"").each(function () {{
                    $(this)[0].checked = result;
                }});
            }}
        </script>
";
        }

        private static string CreateJsOperation()
        {
            StringBuilder content = new StringBuilder(@"

        <script type=""text/javascript"">");

            #region edit
            var editModel = PageCache.GetCmd("编辑");
            if (editModel != null)
            {
                // 定义变量
                StringBuilder editDefineVarContent = new StringBuilder();
                editDefineVarContent.AppendFormat(@"var txtEdit{0} = """";", PageCache.KeyId);

                // 读取变量值
                StringBuilder editReadToVarContent = new StringBuilder();
                editReadToVarContent.AppendFormat(@"txtEdit{0} = $(this).val();", PageCache.KeyId);

                // 变量值 赋值到 内容中
                StringBuilder editVarToUIContent = new StringBuilder();
                editVarToUIContent.AppendFormat(@"$(""#txtEdit{0}"").val(txtEdit{0});", PageCache.KeyId);

                // 初始化提交的参数
                StringBuilder editSubmitContent = new StringBuilder();
                editSubmitContent.AppendFormat(@"var txtEdit{0} = $(""#txtEdit{0}"").val();", PageCache.KeyId);

                // 提交的post字符串内容
                StringBuilder editpostDataContent = new StringBuilder("var postData = ");
                editpostDataContent.AppendFormat(@"""txtEdit{0}="" + encodeURI(txtEdit{0}) ", PageCache.KeyId);

                int index = 0;
                foreach (var item in editModel.AttrList)
                {
                    string attribute = item.AttrName;
                    editDefineVarContent.AppendFormat(@"
                var txtEdit{0} = """";", attribute);

                    editReadToVarContent.AppendFormat(@"
                        txtEdit{0} = $(this).attr(""tag_{0}"");", attribute);

                    editVarToUIContent.AppendFormat(@"
                    $(""#txtEdit{0}"").val(txtEdit{0});", attribute);

                    editSubmitContent.AppendFormat(@"
                var txtEdit{0} = $(""#txtEdit{0}"").val();", attribute);

                    editpostDataContent.AppendFormat(@" + ""&txtEdit{0}="" + encodeURI(txtEdit{0})", attribute);

                    index++;
                }

                if (editpostDataContent.ToString() == "var postData = ")
                {
                    editpostDataContent.Append("\"\"");
                }

                editpostDataContent.Append(";");

                content.AppendFormat(@"

            function editModel() {{
                {0}
                var checkCount=0;
                $(""#tbcontent tbody"").find(""input[type='checkbox']"").each(function () {{
                    if ($(this)[0].checked) {{
                        {1}
                        checkCount++;
                    }}
                }});

                if (checkCount == 0) {{
                    alert(""请选择一个进行编辑！"");
                }} else if (checkCount > 1) {{
                    alert(""只能选择一个进行编辑！"");
                }} else {{
                    {2}
                    $(""#edit_Modal"").modal('show');
                }}
            }}

            function saveEditModel() {{
                {3}
                {4}
                $.ajax({{
                    type: ""POST"",
                    url: ""{5}.aspx?type=edit"",
                    data: postData.replace(""+"", ""%2b""), // 解决加号在传递中变成%20问题
                    success: function (msg) {{
                        if (msg == ""0"") {{
                            alert(""保存成功！"");
                            loadData();
                        }} else {{
                            alert(msg);
                        }}

                        $('#edit_Modal').modal('hide');        // close the dialog
                    }}
                }});
            }}", editDefineVarContent.ToString(),
                    editReadToVarContent.ToString(),
                    editVarToUIContent.ToString(),
                    editSubmitContent.ToString(),
                    editpostDataContent.ToString(),
                    PageCache.TableName_UI);
            }
            #endregion

            #region add
            var addModel = PageCache.GetCmd("添加");
            if (addModel != null)
            {
                StringBuilder addContent = new StringBuilder();
                StringBuilder addPostDataContent = new StringBuilder("var postData = ");
                int index = 0;
                foreach (var item in addModel.AttrList)
                {
                    string attribute = item.AttrName;
                    addContent.AppendFormat(@"
                var txtAdd{0} = $(""#txtAdd{0}"").val();", attribute);

                    if (index == 0)
                    {
                        addPostDataContent.AppendFormat("\"txtAdd{0}=\" + encodeURI(txtAdd{0}) ", attribute);
                    }
                    else
                    {
                        addPostDataContent.AppendFormat(" + \"&txtAdd{0}=\" + encodeURI(txtAdd{0})", attribute);
                    }

                    index++;
                }

                if (addPostDataContent.ToString() == "var postData = ")
                {
                    addPostDataContent.Append("\"\"");
                }

                addPostDataContent.Append(";");

                content.AppendFormat(@"

            function newModel() {{
                $('#add_Modal').modal('show');
            }}

            function saveAddModel() {{
                {0}
                {1}
                $.ajax({{
                    type: ""POST"",
                    url: ""{2}.aspx?type=add"",
                    data: postData.replace(""+"",""%2b""),
                    success: function (msg) {{
                        if (msg == ""0"") {{
                            alert(""添加成功！"");
                            loadData();
                        }} else {{
                            alert(msg);
                        }}

                        $('#add_Modal').modal('hide');
                    }}
                }});
            }}", addContent.ToString(),
               addPostDataContent.ToString(),
               PageCache.TableName_UI);
            }
            #endregion

            #region del
            var deleteModel = PageCache.GetCmd("删除");
            if (deleteModel != null)
            {
                string template = string.Format(@"

            function destroyModel() {{
                var checkCount=0;
                $(""#tbcontent tbody"").find(""input[type='checkbox']"").each(function () {{
                    if ($(this)[0].checked) {{
                        checkCount++;
                    }}
                }});

                if (checkCount > 0) {{
                    $.confirm({{
                        content: ""确定删除选中数据吗?"",
                        text: ""确定删除选中数据吗?"",
                        title: ""提示"",
                        confirm: function(button) {{
                            var ids = """";
                            $(""#tbcontent tbody"").find(""input[type='checkbox']"").each(function () {{
                                if ($(this)[0].checked) {{
                                    ids += $(this).val() + "","";
                                }}
                            }});

                            ids = ids.substring(0, ids.length-1);
                            $.post('{0}.aspx?type=delete', {{ ids: ids }}, function (msg) {{
                                if (msg == ""0"") {{
                                    alert(""删除成功"");
                                    loadData();
                                }} else {{
                                    alert(msg);
                                }}
                            }}, 'json');
                        }},
                        cancel: function(button) {{
                            // nothing to do
                        }},
                        confirmButton: ""确定"",
                        cancelButton: ""取消"",
                        post: true,
                        confirmButtonClass: ""btn-danger"",
                        cancelButtonClass: ""btn-default"",
                        dialogClass: ""modal-dialog modal-lg"" // Bootstrap classes for large modal
                    }});
                }}else{{
                    alert(""请勾选进行删除！"");
                }}
            }}", PageCache.TableName_UI,
                PageCache.KeyId);
                content.Append(template);
            }
            #endregion

            #region bat edit
            var batEdit = PageCache.GetCmd("批量编辑");
            if (batEdit != null)
            {
                string keyAttribute = PageCache.KeyId;

                StringBuilder batEditContent = new StringBuilder();
                batEditContent.AppendFormat("var txtBatEdit{0} = $(\"#txtBatEdit{0}\").val();\r\n", keyAttribute);

                StringBuilder batEditPostDataContent = new StringBuilder("var postData = ");
                batEditPostDataContent.AppendFormat("\"txtBatEdit{0}=\" + encodeURI(txtBatEdit{0}) ", keyAttribute);

                int index = 0;
                foreach (var item in batEdit.AttrList)
                {
                    string attribute = item.AttrName;
                    batEditContent.AppendFormat(@"
                var txtBatEdit{0} = $(""#txtBatEdit{0}"").val();", attribute);
                    batEditPostDataContent.AppendFormat(" + \"&txtBatEdit{0}=\" + encodeURI(txtBatEdit{0})", attribute);

                    index++;
                }

                if (batEditPostDataContent.ToString() == "var postData = ")
                {
                    batEditPostDataContent.Append("\"\"");
                }

                batEditPostDataContent.Append(";");

                content.AppendFormat(@"

            function batEditModel() {{
                var checkCount=0;
                var ids = """";
                $(""#tbcontent tbody"").find(""input[type='checkbox']"").each(function () {{
                    if ($(this)[0].checked) {{
                        ids += $(this).val() + "","";
                        checkCount++;
                    }}
                }});

                if (checkCount>0) {{
                    ids = ids.substring(0, ids.length - 1);
                    $(""#txtBatEdit{0}"").val(ids);
                    $(""#batEdit_Modal"").modal('show');
                }}
            }}

            function saveBatEditModel() {{
                {1}
                {2}
                $.ajax({{
                    type: ""POST"",
                    url: ""{3}.aspx?type=batedit"",
                    data: postData.replace(""+"",""%2b""),
                    success: function (msg) {{
                        if (msg == ""0"") {{
                            alert(""修改成功！"");
                            loadData();
                        }} else {{
                            alert(msg);
                        }}

                        $(""#batEdit_Modal"").modal('hide');      // close the dialog
                    }}
                }});
            }}", PageCache.KeyId,
                    batEditContent.ToString(),
                    batEditPostDataContent.ToString(),
                    PageCache.TableName_UI);
            }

            #endregion

            #region export all

            var exportall = PageCache.GetCmd("导出全部");
            if (exportall != null)
            {
                int index = 0;
                StringBuilder coditionContent = new StringBuilder();
                StringBuilder pagePost = new StringBuilder();
                foreach (var item in exportall.AttrList)
                {
                    string attribute = item.AttrName;

                    // 找到一个
                    coditionContent.AppendFormat(@"
                var txtSearch{0} = $(""#txtSearch{0}"").val();", attribute);

                    // var pagePost = "&xxx=" + xxx + "&yyy=" +yyy;
                    if (index == 0)
                    {
                        pagePost.AppendFormat("var pageData = \"&txtSearch{0}=\" + txtSearch{0}", attribute);
                    }
                    else
                    {
                        pagePost.AppendFormat("+ \"&txtSearch{0}=\" + txtSearch{0}", attribute);
                    }

                    index++;
                }

                if (exportall.AttrList.Count == 0)
                {
                    pagePost.Append("var pageData = \"\";\r\n");
                }
                else
                {
                    pagePost.Append(";");
                }

                // selectExport
                content.AppendFormat(@"

            function exportAll() {{
                {1}
                {2}
                document.location=""{0}.aspx?type=downall"" + pageData.replace(""+"",""%2b"");
            }}", PageCache.TableName_UI,
               coditionContent.ToString(),
               pagePost.ToString());
            }

            #endregion

            #region export part

            var exportselect = PageCache.GetCmd("导出选中");
            if (exportselect != null)
            {
                int index = 0;
                StringBuilder coditionContent = new StringBuilder();
                StringBuilder pagePost = new StringBuilder();
                foreach (var item in exportselect.AttrList)
                {
                    string attribute = item.AttrName;

                    // 找到一个
                    coditionContent.AppendFormat(@"
                var txtSearch{0} = $(""#txtSearch{0}"").val();", attribute);

                    // var pagePost = "&xxx=" + xxx + "&yyy=" +yyy;
                    if (index == 0)
                    {
                        pagePost.AppendFormat("var pageData = \"&txtSearch{0}=\" + txtSearch{0}", attribute);
                    }
                    else
                    {
                        pagePost.AppendFormat("+ \"&txtSearch{0}=\" + txtSearch{0}", attribute);
                    }

                    index++;
                }

                if (exportselect.AttrList.Count == 0)
                {
                    pagePost.Append("var pageData = \"\";\r\n");
                }
                else
                {
                    pagePost.Append(";");
                }

                // selectExport
                content.AppendFormat(@"

            function selectExport() {{
                var checkCount=0;
                var ids = """";
                $(""#tbcontent tbody"").find(""input[type='checkbox']"").each(function () {{
                    if ($(this)[0].checked) {{
                        ids += $(this).val() + "","";
                        checkCount++;
                    }}
                }});

                if (checkCount>0) {{
                    document.location=""{0}.aspx?type=down&ids="" + ids;
                }}
            }}
", PageCache.TableName_UI);
            }

            #endregion

            content.AppendLine(@"
        </script>");

            return content.ToString();
        }

        private static string CreateJsLoad()
        {
            StringBuilder jsContent = new StringBuilder();
            string template = @"

        <script type=""text/javascript"">
            var hpageSize = 13;
            var hpage = 1;
            $(document).ready(function () {{
                loadData(hpage, hpageSize);
                
                if (($(""#btnsearch"")!=null && $(""#btnsearch"")!=undefined)){{
                    $(""#btnsearch"").click(function () {{ return loadData(hpage, hpageSize); }});
                }}
            }});

            function loadData(page, pageSize) {{
                if (page == undefined) {{
                    page = hpage;
                }}

                if (pageSize == undefined) {{
                    pageSize = hpageSize;
                }}

                hpage = page;
                hpageSize = pageSize;
                $(""#tbcontent tbody"").empty();
                $(""#tbcontent tbody"").append(""<tr><td colspan='{0}'>内容加载中...</td></tr>"");
                {1}
                {2}
                var postData = ""page="" + page + ""&pageSize="" + pageSize + pageData;
                $.ajax({{
                    type: ""POST"",
                    url: ""{3}.aspx?type=loaddata"",
                    data: postData.replace(""+"",""%2b""),
                    success: function (text) {{
                        var data = eval(""("" + text + "")"");
                        var content = """";
                        for (var index in data.Data) {{
                            var model = data.Data[index];
                            content += ""<tr>"";
                            {4}
                            content += ""</tr>"";
                        }}

                        $(""#tbcontent tbody"").empty();
                        $(""#tbcontent tbody"").append(content);

                        var selectSize13 = """";
                        var selectSize50 = """";
                        var selectSize100 = """";
                        var selectSize500 = """";
                        var selectSize1000 = """";
                        if (hpageSize == 13) {{
                            selectSize13 = "" selected=\""selected\"" "";
                        }} else if (hpageSize == 50) {{
                            selectSize50 = "" selected=\""selected\"" "";
                        }} else if (hpageSize == 100) {{
                            selectSize100 = "" selected=\""selected\"" "";
                        }} else if (hpageSize == 500) {{
                            selectSize500 = "" selected=\""selected\"" "";
                        }} else if (hpageSize == 1000) {{
                            selectSize1000 = "" selected=\""selected\"" "";
                        }}

                        var pageSizeSelect = ""<select class=\""pull-left\"" style=\""display:inline; height:34px; margin:20px;\"" id=\""div_pagesize\"" onchange=\""javascript:changesize();\""><option "" + selectSize13 + "" value=\""13\"">13</option><option "" + selectSize50 + "" value=\""50\"">50</option><option "" + selectSize100 + "" value=\""100\"">100</option><option "" + selectSize500 + "" value=\""500\"">500</option><option "" + selectSize1000 + "" value=\""1000\"">1000</option></select>"";

                        // 构造页码
                        var pageContent = pageSizeSelect + ""<ul class=\""pagination\""><li><a href=\""javascript:loadData(1,"" + pageSize + "");\"">&laquo;</a></li>"";
                        var startI = 1;
                        var endI = 1;
                        if (data.PageCount > 5) {{
                            if ((page - 2) >= 1 && (page + 2) <= data.PageCount) {{
                                startI = page - 2;
                                endI = page + 2;
                            }}
                            else if ((page - 2) < 1) {{
                                startI = 1;
                                endI = 5;
                            }}
                            else {{
                                startI = (data.PageCount - 4);
                                endI = data.PageCount;
                            }}
                        }} else {{
                            startI = 1;
                            endI = data.PageCount;
                        }}

                        for (var i = startI; i <= endI; i++) {{
                            if (page == i) {{
                                pageContent += ""<li class=\""active\""><a href=\""javascript:void();\"">"" + i + ""</a></li>"";
                            }}
                            else {{
                                pageContent += ""<li><a href=\""javascript:loadData("" + i + "","" + pageSize + "");\"">"" + i + ""</a></li>"";
                            }}
                        }}

                        pageContent += ""<li><a href=\""javascript:loadData("" + data.PageCount + "","" + pageSize + "");\"">&raquo;</a></li></ul>"";
                        $(""#msgPager"").empty();
                        $(""#msgPager"").append(pageContent);
                    }}
                }});
            }}

            function changesize() {{
                var size = $(""#div_pagesize"").val();
                hpageSize = size;
                loadData(hpage, hpageSize);
            }}
        </script>
";

            int index = 0;
            StringBuilder coditionContent = new StringBuilder();
            StringBuilder pagePost = new StringBuilder();
            var showModel = PageCache.GetCmd("主显示");
            if (showModel != null)
            {
                foreach (var item in showModel.AttrList)
                {
                    string attribute = item.AttrName;

                    // 找到一个
                    coditionContent.AppendFormat(@"
                var txtSearch{0} = $(""#txtSearch{0}"").val();", attribute);

                    if (index == 0)
                    {
                        pagePost.AppendFormat("var pageData = \"&txtSearch{0}=\" + txtSearch{0}", attribute);
                    }
                    else
                    {
                        pagePost.AppendFormat("+ \"&txtSearch{0}=\" + txtSearch{0}", attribute);
                    }

                    index++;
                }

                // 用于编辑使用扩展属性
                StringBuilder tagContent = new StringBuilder();
                tagContent.Append(@"var attr = """);
                index = 0;
                foreach (var item in showModel.AttrList)
                {
                    var attribute = item.AttrName;
                    if (index == 0)
                    {
                        tagContent.AppendFormat(@" tag_{0}='"" + model.{0} + ""'"" ", attribute);
                    }
                    else
                    {
                        tagContent.AppendFormat(@" + "" tag_{0}='"" + model.{0} + ""'"" ", attribute);
                    }

                    index++;
                }

                if (index == 0)
                {
                    tagContent.Append(@""";");
                }
                else
                {
                    tagContent.Append(";");
                }

                StringBuilder showContent = new StringBuilder();
                showContent.AppendFormat(@"
                            {1}
                            content += ""<td><input type=\""checkbox\"" id=\""chk"" + model.{0} + ""\"" value=\"""" + model.{0} + ""\"" "" + attr + "" /></td>"";", PageCache.KeyId, tagContent.ToString());
                foreach (var item in showModel.AttrList)
                {
                    showContent.AppendFormat(@"
                            content += ""<td>"" + model.{0} + ""</td>"";", item.AttrName);
                }

                int colspanCount = showModel.AttrList.Count;
                pagePost.Append(";");
                if (showModel.AttrList.Count == 0)
                {
                    pagePost.Append("var pageData = \"\";\r\n");
                }

                jsContent.AppendFormat(template,
                    colspanCount,
                    coditionContent,
                    pagePost,
                    PageCache.TableName_UI,
                    showContent.ToString());

                return jsContent.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        private static string CreateBottomContent()
        {
            return @"
    </div>
</body>
</html>";
        }
    }
}
