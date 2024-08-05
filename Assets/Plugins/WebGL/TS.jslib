var plugin = {
    InvoiceInternal: function(url)
    {
        if (window && window.Telegram && window.Telegram.WebApp){
            window.Telegram.WebApp.openInvoice(UTF8ToString(url));
        }
        else{
            window.alert(Pointer_stringify('Telegram not found'));
        }
    },
    RequestUserData: function () {
        if (window.unityInstance) {
            window.unityInstance.SendMessage("TelegramController", "SetWebAppUser", JSON.stringify(window.Telegram.WebApp.initDataUnsafe.user));
        }
    },
};
mergeInto(LibraryManager.library, plugin);
/*
mergeInto(LibraryManager.library, {

    Hello: function () {
        window.alert("Hello, world!");
    },

    HelloString: function (str) {
        window.alert(Pointer_stringify(str));
    },

    PrintFloatArray: function (array, size) {
        for(var i = 0; i < size; i++)
            console.log(HEAPF32[(array >> 2) + size]);
    },

    StringReturnValueFunction: function () {
        var returnStr = "bla";
        var buffer = _malloc(lengthBytesUTF8(returnStr) + 1);
        writeStringToMemory(returnStr, buffer);
        return buffer;
    },

    BindWebGLTexture: function (texture) {
        GLctx.bindTexture(GLctx.TEXTURE_2D, GL.textures[texture]);
    },

});*/
