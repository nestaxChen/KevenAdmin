/**
 * web-upload 工具类
 *
 * 上传按钮的id = 图片隐藏域id + 'BtnId'
 *
 */
(function() {

	var $WebUpload = function (pictureId) {
		this.pictureId = pictureId;
		this.uploadBtnId = pictureId + 'BtnId';
		this.uploadUrl = '/interface/fileUpload.ashx';
		this.fileSizeLimit = 10 * 1024 * 1024;
		this.filetype = pictureId;
		this.forfile = $('#' + pictureId + 'BtnId').data('forfile');
		this.orderid = $('#' + pictureId + 'BtnId').data('orderid');
		if (this.orderid || this.forfile) {
			this.formData = {
				forfile: this.forfile,
				orderid: this.orderid
			}
		} else {
			this.formData = {
				Isfile: 'file',
				FileType: this.filetype
			}
		}
	};

	$WebUpload.prototype = {
		/**
		 * 初始化webUploader
		 */
		init : function() {
			var uploader = this.create();
			this.bindEvent(uploader);
			return uploader;
		},

		/**
		 * 创建webuploader对象
		 */
		create : function() {
			var webUploader = WebUploader.create({
				auto : true,
				pick : {
					id : '#' + this.uploadBtnId,
					multiple : false,// 只上传一个
				},
				accept : {
					title : 'Images',
					extensions : 'gif,jpg,jpeg,bmp,png',
					mimeTypes : 'image/gif,image/jpg,image/jpeg,image/bmp,image/png'
				},
				fileVal: 'file1',
				formData: this.formData,
				swf : '../Uploader.swf',
				disableGlobalDnd : true,
				duplicate : true,
				server : this.uploadUrl,
				fileSingleSizeLimit : this.fileSizeLimit
			});

			return webUploader;
		},

		/**
		 * 绑定事件
		 */
		bindEvent : function(bindedObj) {
			var me =  this;

			// 文件上传过程中创建进度条实时显示。
			bindedObj.on('uploadProgress', function(file, percentage) {
				var $my = $('#' + me.uploadBtnId),
						$percent = $my.find('.progress span');

				// 避免重复创建
				if (!$percent.length) {
					$percent = $('<p class="progress"><span></span></p>')
						.appendTo($my)
						.find('span');
				}

				$percent.css('width', percentage * 100 + '%');
				$('#' + me.pictureId + 'Img').attr('src', '/Content/Img/loading.gif');
			});

			// 文件上传成功
			bindedObj.on('uploadSuccess', function(file, response) {
				if (response.status_code !== '0') {
                    layer.alert(response.message, {
                        skin: 'layui-layer-lan'
                        , closeBtn: 0
                        , anim: 4 //动画类型
                    });
					$('#' + me.pictureId + 'Img').attr('src', '/Content/Img/upload_big.png');
					return false;
				}

				$('#' + me.pictureId).val(response.data.filename);
				$('#' + me.pictureId + 'Img').attr('src', response.data.filename);

				var html = '<span class="reload-txt">重新上传</span>'
				$('#' + me.uploadBtnId).find('.webuploader-pick').append(html);

			});

			// 文件上传失败，显示上传出错。
			bindedObj.on('uploadError', function(file) {
                layer.alert('上传失败！', {
                    skin: 'layui-layer-lan'
                    , closeBtn: 0
                    , anim: 4 //动画类型
                });

				$('#' + me.pictureId + 'Img').attr('src', '/Content/Img/upload_big.png');
			});

			// 其他错误
			bindedObj.on('error', function(type) {
				if ('F_EXCEED_SIZE' == type) {
                    layer.alert('请将文件大小控制在10M以内！', {
                        skin: 'layui-layer-lan'
                        , closeBtn: 0
                        , anim: 4 //动画类型
                    });
				} else if ('Q_TYPE_DENIED' == type) {
                    layer.alert('文件类型不满足！', {
                        skin: 'layui-layer-lan'
                        , closeBtn: 0
                        , anim: 4 //动画类型
                    });
				} else if ('Q_EXCEED_NUM_LIMIT' == type) {
                    layer.alert('上传数量超过限制！', {
                        skin: 'layui-layer-lan'
                        , closeBtn: 0
                        , anim: 4 //动画类型
                    });
				} else if ('F_DUPLICATE' == type) {
                    layer.alert('图片选择重复！', {
                        skin: 'layui-layer-lan'
                        , closeBtn: 0
                        , anim: 4 //动画类型
                    });
				} else {
                    layer.alert('上传过程中出错！', {
                        skin: 'layui-layer-lan'
                        , closeBtn: 0
                        , anim: 4 //动画类型
                    });
				}
			});

			// 完成上传完了，成功或者失败
			bindedObj.on('uploadComplete', function(file) {
				$('#' + me.uploadBtnId).find('.progress').remove();
			});
		}
	};

	window.$WebUpload = $WebUpload;

}());