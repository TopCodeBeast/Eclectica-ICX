context {
    input endpoint: string;
    input name: string;
}

start node root {
    do {
        #connectSafe($endpoint);
        #waitForSpeech(1000);
        #sayText("Hello " + $name);
        #sayText("Welcome to eclectica metaverse!");
        #sayText("My name is kate, I can help you to learn about blockchain. What you want to know?");
        wait *;
    }
}

digression blockchain
{
    conditions {on #messageHasIntent("blockchain");}
    do 
    {
      #say("blockchain_i");
      wait *;
    }
}


digression nft
{
    conditions {on #messageHasIntent("nft");}
    do 
    {
      #say("nft_i");
      wait *;
    }
}

digression Defi
{
    conditions {on #messageHasIntent("defi");}
    do 
    {
       #say("defi_i");
       wait *;
    }
}

digression Dao
{
    conditions {on #messageHasIntent("dao");}
    do 
    {
       #say("dao_i");
       wait *;
    }
}



