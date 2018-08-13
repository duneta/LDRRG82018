using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class NovelBranch
{
	public string title;
	public List<NovelFrame> frames;
	public NovelBranch(string title, List<NovelFrame> frames)
	{
		this.title = title;
		this.frames = frames;
	}
}

public abstract class NovelFrame
{
	public Tokens token;
	public object payload;
	public override string ToString()
	{
		return  token.ToString()+":" + payload.ToString();
	}

}

public class NovelOption : NovelFrame
{
	public override string ToString()
	{
		return  token.ToString()+"=>("+target+"):" + payload.ToString();
	}

	public string title;
	public string target;
	public NovelOption(ref List<FrameFragment> fragments)
	{ 
		token = Tokens.option;
		payload = fragments[0].payload;
		fragments.Remove(fragments[0]);

		target = (string) new NovelJump(ref fragments).payload;
	}
}

public class NovelText: NovelFrame
{
	public override string ToString()
	{
		return token.ToString()+":"+character+":"+line;
	}
	public string character;
	public string line;
	public NovelText(ref List<FrameFragment> fragments)
	{
		token = Tokens.TEXT;
		fragments.Remove(fragments[0]);

		NovelCharacter novelCharacter = new NovelCharacter(ref fragments);

		string line = string.Empty;
		foreach (NovelFrame frame in novelCharacter.ownedFrames)
		{
			line += frame.payload;
		}

		this.character = novelCharacter.name;
		this.line = line;
	}
}

public class NovelJump : NovelFrame
{
	public NovelJump(ref List<FrameFragment> fragments)
	{ 
		token = Tokens.jump;
		payload = fragments[0].payload;
		fragments.Remove(fragments[0]);
	}
}

public class NovelHideCharacter : NovelFrame
{
	public NovelHideCharacter(ref List<FrameFragment> fragments)
	{ 
		token = Tokens.hidecharacter;
		payload = fragments[0].payload;
		fragments.Remove(fragments[0]);
	}
}

public class NovelDirective : NovelFrame
{
	public NovelDirective(ref List<FrameFragment> fragments)
	{ 
		token = Tokens.directive;
		payload = fragments[0].payload;
		fragments.Remove(fragments[0]);
	}
}

public class NovelOptions : NovelFrame
{
	public List<NovelOption> options;

	public override string ToString()
	{
		string result = token.ToString()+":\n";
		foreach (NovelOption option in options)
		{result += option.ToString() + "\n";}
		return result;
	}
	public NovelOptions(ref List<FrameFragment> frames)
	{
		token = Tokens.options;
		frames.Remove(frames[0]);
		options = new List<NovelOption>();

		List<FrameFragment> selected = new List<FrameFragment>();
		List<FrameFragment> post = new List<FrameFragment>();

		bool foundExit = false;
		foreach (FrameFragment fragment in frames)
		{
			if (fragment.token == Tokens.exitbranch)
			{ throw new System.Exception("Reached the end of a branch before an option tree was terminated.");}

			if (fragment.token == Tokens.endoptions)
			{
				foundExit = true;
			}
			else
			{
				if (foundExit)
				{ post.Add(fragment); }
				else 
				{ selected.Add(fragment); }
			}
		}

		if (foundExit == false)
		{
			 throw new System.Exception("Expected token to terminate option tree "
			+" but did not find terminator endoptions."); 
		}

		while (selected.Count > 0)
		{
			options.Add(new NovelOption(ref selected));
		}

		frames = post;

	}
}

public class NovelMonologue : NovelFrame
{
	public NovelMonologue(ref List<FrameFragment> lines)
	{
		token = Tokens.monologue;

		List<FrameFragment> selected = new List<FrameFragment>();
		List<FrameFragment> post = new List<FrameFragment>();
		bool terminator = false;
		string payload = string.Empty;

		lines.Remove(lines[0]);

		foreach (FrameFragment fragment in lines)
		{ 
			if (fragment.token != Tokens.none)
			{
				terminator = true;
				post.Add(fragment);
			}
			else
			{
				if (terminator)
				{
					post.Add(fragment);
				}
				else 
				{
					selected.Add(fragment);
					if (payload != string.Empty)
					{payload += " ";}
					payload += fragment.payload;
				}
			}
		}

		this.payload = payload;
		lines = post;
	}
}

public class NovelLine : NovelFrame
{
	public string character;
	public NovelLine(ref List<FrameFragment> lines, NovelCharacter character)
	{
		this.character = character.name;
		token = Tokens.line;

		List<FrameFragment> selected = new List<FrameFragment>();
		List<FrameFragment> post = new List<FrameFragment>();
		bool terminator = false;
		string payload = lines[0].payload;

		lines.Remove(lines[0]);

		foreach (FrameFragment fragment in lines)
		{ 
			if (fragment.token != Tokens.none)
			{
				terminator = true;
				post.Add(fragment);
			}
			else
			{
				if (terminator)
				{
					post.Add(fragment);
				}
				else 
				{
					selected.Add(fragment);
					if (payload != string.Empty)
					{payload += " ";}
					payload += fragment.payload;
				}
			}
		}

		this.payload = payload;
		lines = post;
	}
}



public class NovelBackground : NovelFrame
{
	public NovelBackground(ref List<FrameFragment> fragments)
	{ 
		token = Tokens.background;
		payload = fragments[0].payload;
		fragments.Remove(fragments[0]);
	}
}

public class NovelExpression : NovelFrame
{
	public string character;
	public NovelExpression(ref List<FrameFragment> fragments, NovelCharacter character)
	{ 
		this.character = character.name;
		token = Tokens.expression;
		payload = fragments[0].payload;
		fragments.Remove(fragments[0]);
	}
}

public class NovelCharacter : NovelFrame
{
	public override string ToString()
	{
		string result = token.ToString() + "("+name+")";
		if (ownedFrames != null)
		{
			result+="\n";
			foreach (NovelFrame frame in ownedFrames)
			{
				result += frame.ToString() + "\n";
			}
		}
		return result;
	}
	public string name;
	public List<NovelFrame> ownedFrames;
	public List<NovelFrame> Flatten()
	{
		List<NovelFrame> frames = ownedFrames;
		ownedFrames = null;
		frames.Insert(0, this);
		return frames;
	}
	public NovelCharacter(ref List<FrameFragment> fragments)
	{
		token = Tokens.character;
		name = fragments[0].payload;
		ownedFrames = new List<NovelFrame>();

		bool terminator = false;

		fragments.Remove(fragments[0]);

		while (!terminator && fragments.Count > 0)
		{ 
			switch (fragments[0].token)
			{
				case Tokens.expression:
				{
					ownedFrames.Add(new NovelExpression(ref fragments, this));
				} break;
				case Tokens.line:
				{
					ownedFrames.Add(new NovelLine(ref fragments, this));
				} break;
				default:
				{
					terminator = true;
				} break;
			}
		}
	}
}


public enum Tokens { none, monologue, background, branch, character, hidecharacter, 
		expression, line, jump, exitbranch, directive, TEXT,
		options, option, optionexit, endoptions}

public class NovelScript
{
	Dictionary<string, List<NovelFrame>> branches;

}

public class FrameFragment
{
	public Tokens token;
	public string payload;

}

public class ParseScript {

	ParseState state = ParseState.none;

	[System.Flags]
	public enum ParseState { none=0, branch, character, line, options, option}

	// Use this for initialization
	public List<NovelBranch> LoadScript () {
		//StreamReader reader = new StreamReader("Assets/Resources/LDSCRIPTRRG.script");
		StringReader reader = new StringReader(TheDAta.rawDAta);

		List<FrameFragment> lines = new List<FrameFragment>();
		
		while (reader.Peek() != -1)
		{
			string outLine = string.Empty;
			string line = reader.ReadLine();
			line = line.TrimStart(' ','\t').TrimEnd(' ','\t');
			if (line.StartsWith("//") || line == string.Empty || line.StartsWith("endline"))
			{ 
				outLine = "COMMENT: " + line;
			}
			else
			{ 
				lines.Add(Parse(line));
				outLine = line;
			}
		}

		List<NovelBranch> branches = new List<NovelBranch>();
		while (lines.Count > 0)
		{ 
			NovelBranch branch = ConsumeBranch(ref lines);
			string log = "BRANCH:"+branch.title+"\n";
			foreach (NovelFrame frame in branch.frames)
			{
				log += frame + "\n";
			}
			//Debug.Log(log);
			branches.Add(branch);
		}
		return branches;
	}
	
	FrameFragment Parse(string line)
	{
		//Debug.Log("PARSE("+state+"):"+line);
		string payload = string.Empty;
		Tokens t = Tokens.none;
		foreach (int i in System.Enum.GetValues(typeof(Tokens)))
		{
			
			Tokens tIndex = (Tokens)i;
			if (line.StartsWith(tIndex.ToString())
			|| (tIndex == Tokens.monologue && line.StartsWith("monolouge")))
			{
				t = tIndex;
				payload = line.Substring(tIndex.ToString().Length).TrimStart(' ','\t').TrimEnd(' ','\t');
				
				break;
			}
		}

		if (t == Tokens.none)
		{payload = line;}
		//Debug.Log(t.ToString() + "|" + payload);
		
		return new FrameFragment() {token = t, payload = payload};
	}

	NovelBranch ConsumeBranch(ref List<FrameFragment> lines)
	{
		List<NovelFrame> result = new List<NovelFrame>();

		if (lines[0].token != Tokens.branch)
		{ throw new System.Exception("Expected TOKEN branch but got "+lines[0].token); }

		string title = lines[0].payload;

		lines.Remove(lines[0]);

		List<FrameFragment> frames = new List<FrameFragment>();
		List<FrameFragment> post = new List<FrameFragment>();

		bool foundExit = false;

		foreach (FrameFragment fragment in lines)
		{
			if (foundExit)
			{ post.Add(fragment); }
			else 
			{ 
				if (fragment.token == Tokens.exitbranch)
				{ foundExit = true; }
				else 
				{ frames.Add(fragment);} 
			}
		}

		if (foundExit == false)
		{
			 throw new System.Exception("Expected token to terminate Branch "+title
			+" but did not find terminator. ("
			+("exitbranch "+title)+")"); 
		}

		while (frames.Count > 0)
		{ 
			NovelFrame frame = Consume(ref frames);
			if (frame is NovelCharacter)
			{
				List<NovelFrame> flattened = (frame as NovelCharacter).Flatten();
				result.AddRange(flattened);
			} 
			else
			{
				result.Add( frame);
			}
		}

		lines = post;
		return new NovelBranch(title, result);
	}

	NovelFrame Consume(ref List<FrameFragment> fragments)
	{
		switch (fragments[0].token)
		{
			case Tokens.background:
			{ 
				return new NovelBackground(ref fragments);
			} 
			case Tokens.monologue:
			{ 
				return new NovelMonologue(ref fragments);
			} 
			case Tokens.character:
			{
				return new NovelCharacter(ref fragments);
			} 
			case Tokens.options:
			{
				return new NovelOptions(ref fragments);
			} 
			case Tokens.hidecharacter:
			{
				return new NovelHideCharacter(ref fragments);
			} 
			case Tokens.directive:
			{
				return new NovelDirective(ref fragments);
			}
			case Tokens.jump:
			{ 
				return new NovelJump(ref fragments); 
			}
			case Tokens.TEXT:
			{
				return new NovelText(ref fragments);
			}
			
 			default: 
			{
				throw new System.IndexOutOfRangeException("Bad token:"+fragments[0].token.ToString());
			}
		}
	}

}
