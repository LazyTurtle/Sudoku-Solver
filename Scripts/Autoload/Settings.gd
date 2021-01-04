extends Node

export(Theme) var global_theme = preload("res://Assets/UI/pixel theme/pixel theme high resx4.tres")

enum property {THEME}

var settings := Dictionary()

func _enter_tree():
	settings[property.THEME]=global_theme
	pass

func get_setting_property(setting:int):
	assert(setting in property.values(), "Only Settings.peoperty enum are allowed")
	return settings[setting]
