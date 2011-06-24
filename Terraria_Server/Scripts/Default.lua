function PlayerCommandHandler(Command)
	if Command == "player_lua" then
		PrintToConsole("Received command ".. Command)
		PrintToChat("Received command ".. Command)
		return "#cancel"
	end
end

function ConsoleCommandHandler(Command)
	if Command == "console_lua" then
		PrintToConsole("Received command ".. Command)
		PrintToChat("Received command ".. Command)
		return "#cancel"
	end
end