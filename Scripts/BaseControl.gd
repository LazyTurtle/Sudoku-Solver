extends Control

export(bool) var has_custom_theme = false

func _enter_tree():
	if(!has_custom_theme):
		theme = Settings.get_setting_property(Settings.property.THEME)
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
