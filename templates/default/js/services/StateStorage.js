function StateStorage(){
	this.bag = {};	
}

StateStorage.prototype.put = function(key, value){
	this.bag[key] = value;
};

StateStorage.prototype.get = function(key){
	return this.bag[key];
};

module.exports = StateStorage;