# Montefiore

## How We Use It
Visit the website

![alt home](https://github.com/roi-becker/Montefiore/blob/master/README%20assets/home.png)

Choose your appartement and enter your password

![alt password](https://github.com/roi-becker/Montefiore/blob/master/README%20assets/password.png)

Select the type of machine and duration you need

![alt select](https://github.com/roi-becker/Montefiore/blob/master/README%20assets/select.png)

Confirm your selection

![alt confirm](https://github.com/roi-becker/Montefiore/blob/master/README%20assets/confirm.png)

The machine you selected will now be powered on for the duration you specified (plus a few extra minutes on the house).

Once every couple of months, we gather the usage statistics for each appartement and bill them accordingly (to pay for this whole operation).

## How It Works
Our website keeps tracks of all the users, their passwords, and usage history (SQL).
Inside the building's main power outlet, we have power lines running to a washing and drying machine on each floor.

![alt outlet](https://github.com/roi-becker/Montefiore/blob/master/README%20assets/outlet.jpg)

A raspberry pi (2...) connected to a relay board is controlling all of those power lines.

![alt pi](https://github.com/roi-becker/Montefiore/blob/master/README%20assets/pit.jpg)

Once every few seconds, it will query the website (through a REST API) for the machines that should get power.
Since all communication goes from the client to the server, we don't need to deal with static IP / DNS.
For convinience, we have the computer connected to a screen showing what machines are on.

![alt dashboard](https://github.com/roi-becker/Montefiore/blob/master/README%20assets/dashboard.jpeg)

## How To Deploy

### Important Note
Due to my lack of will to deal with certificates (and my hope that now one will try to hack us), the entire system is not secured.
Specifically, the website use HTTP and the passwords are passed to the server using clear text.
Access to the database is standardly password-protected so this part is fine.