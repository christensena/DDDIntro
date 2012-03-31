var Country = Spine.Model.sub();

Country.configure("Country", "name", "id");

// Persist with Local Storage
Country.extend(Spine.Model.Local);

Country.extend({
  all: function(){
    return this.select(function(item) {
      return true;
    });
  },

  destroyAll: function(){
    var items = this.all();
    for(var i=0; i < items.length; i++)
      items[i].destroy();
  }
});


var CountriesController = Spine.Controller.sub({
    elements: {
        ".items": "items",
        "form input": "input"
    },

    events: {
      "submit form": "create",
      "click .clear": "clear"
    },

    init: function () {
        Country.bind("create", this.proxy(this.addOne));
        Country.bind("refresh", this.proxy(this.addAll));
        Country.fetch();
    },

    addOne: function (country) {
        var view = new CountryController({ item: country });
        this.items.append(view.render().el);
    },

    addAll: function () {
        Country.each(this.proxy(this.addOne));
    },

    create: function (e) {
        e.preventDefault();
        Country.create({ name: this.input.val() });
        this.input.val("");
    },

    clear: function () {
        Country.destroyAll();
    }
});

var CountryController = Spine.Controller.sub({

    elements: {
        "input[type=text]": "input"
    },

    events: {
       "click  .destroy":               "destroyItem",
       "dblclick .view":                "edit",
       "keypress input[type=text]":     "blurOnEnter",
       "blur     input[type=text]":     "close"
    },

    init: function () {
        this.item.bind("update", this.proxy(this.render));
        this.item.bind("destroy", this.proxy(this.remove));
    },

    render: function () {
        this.replace($("#countryTemplate").tmpl(this.item));
        return this;
    },

    remove: function () {
        this.el.remove();
        this.release();
    },

    edit: function () {
        this.el.addClass("editing");
        this.input.focus();
    },

    blurOnEnter: function(e) {
      if (e.keyCode === 13) 
        e.target.blur();
    },

    close: function() {
      this.el.removeClass("editing");
      this.item.updateAttributes({
        name: this.input.val()
      });
    },

    destroyItem: function() {
        this.item.destroy();
    }

});

jQuery(function ($) {
    return new CountriesController({
        el: $("#countries")
    });
});