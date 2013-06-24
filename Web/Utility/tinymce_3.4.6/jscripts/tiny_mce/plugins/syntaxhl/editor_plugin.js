(function() {
    tinymce.PluginManager.requireLangPack('syntaxhl');
    
    tinymce.create('tinymce.plugins.SyntaxHL', {
        init : function(ed, url) {
            ed.addCommand('mceSyntaxHL', function() {
                ed.windowManager.open({
                    file : url + '/syntaxhl.htm',
                    width : parseInt(ed.getParam("plugin_syntaxhl_width", "900")),
                    height : parseInt(ed.getParam("plugin_syntaxhl_height", "600")),
                    inline : ed.getParam("plugin_syntaxhl_inline", 1)
                }, {
                    plugin_url : url // Plugin absolute URL
                });
            });

            ed.addButton('syntaxhl', {
                title : 'syntaxhl.desc',
                cmd : 'mceSyntaxHL',
                image : url + '/code.gif'
            });
        },

        createControl : function(n, cm) {
            return null;
        },

        getInfo : function() {
            return {
                longname : 'Syntax Highlighter',
                author : 'Richard Grundy',
                authorurl : 'http://27smiles.com',
                infourl : 'http://27smiles.com',
                version : "1.0"
            };
        }
    });

    tinymce.PluginManager.add('syntaxhl', tinymce.plugins.SyntaxHL);
})();